using CommonDataLayer.DTO;
using CommonDataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer
{
    public class PaymentDL : BaseDL<Payment>, IPaymentDL
    {
        private readonly CondoContext _condoContext;
        private readonly DbSet<Payment> _dbSet;

        public PaymentDL(CondoContext condoContext) : base(condoContext)
        {
            _condoContext = condoContext;
            _dbSet = condoContext.Set<Payment>();
        }

        public async Task AddPaymentsAsync(List<Payment> payments)
        {
            using var transaction = await _condoContext.Database.BeginTransactionAsync();
            try
            {
                // Thêm danh sách payment
                await _dbSet.AddRangeAsync(payments);
                await _condoContext.SaveChangesAsync();

                // Commit transaction nếu thành công
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                // Rollback nếu có lỗi
                await transaction.RollbackAsync();
                throw; // hoặc log lỗi
            }
        }

        public async Task<List<PaymentReportDto>> GetReportByMonthly(DateTime startDate, DateTime endDate)
        {
            var report = await _dbSet
                    .Where(p => p.PaymentDate >= startDate && p.PaymentDate <= endDate)
                    .GroupBy(p => new { Year = p.PaymentDate.Value.Year, Month = p.PaymentDate.Value.Month })
                    .Select(g => new PaymentReportDto
                    {
                        Year = g.Key.Year,
                        Month = g.Key.Month,
                        TotalAmount = g.Sum(x => x.Amount),
                        Count = g.Count()
                    })
                    .OrderBy(r => r.Year).ThenBy(r => r.Month)
                    .ToListAsync();

            return report;
        }

        public async Task<List<PaymentReportDto>> GetReportByWeekly(DateTime startDate, DateTime endDate)
        {
            var report = _dbSet
                .Where(p => p.PaymentDate >= startDate && p.PaymentDate <= endDate)
                .AsEnumerable()
                .GroupBy(p => new
                {
                    Year = p.PaymentDate.Value.Year,
                    Week = System.Globalization.CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
                            p.PaymentDate.Value,
                            System.Globalization.CalendarWeekRule.FirstDay,
                            DayOfWeek.Monday)
                    //Week = EF.Functions.DatePart("week", p.PaymentDate.Value)
                })
                .Select(g => new PaymentReportDto
                {
                    Year = g.Key.Year,
                    Week = g.Key.Week,
                    TotalAmount = g.Sum(x => x.Amount),
                    Count = g.Count()
                })
                .OrderBy(r => r.Year).ThenBy(r => r.Week)
                .ToList();

            return report;

        }

        public override FilterResult<Payment> FilterData(FilterData filterData)
        {
            var query = _dbSet.AsQueryable();

            query = query
                .Include(x => x.Contract)
                .Include(x => x.Booking)
                .Include(a => a.Resident)
                    .ThenInclude(ax => ax.Account);

            if (filterData.Conditions.Any())
            {
                foreach (var condition in filterData.Conditions)
                {
                    if (condition.Key == "PaymentMethod")
                    {
                        query = query.Where(r => r.PaymentMethod == condition.PaymentStatusValue);
                    }
                    else if (condition.GuidValue != null)
                    {
                        var lambda = CreateLambda(condition.Key, condition.GuidValue);
                        query = query.Where(lambda);
                    }
                    else
                    {
                        var lambda = CreateLambda(condition.Key, condition.Value, "Contains");
                        query = query.Where(lambda);
                    }
                }
            }

            FilterResult<Payment> filterResult = new FilterResult<Payment>();

            filterResult.TotalRecords = query.Count();

            if (filterData.Page != 0)
            {
                query = query.Skip((filterData.Page - 1) * filterData.Limit).Take(filterData.Limit);
            }

            filterResult.Results = query.ToList();

            return filterResult;
        }
    }
}
