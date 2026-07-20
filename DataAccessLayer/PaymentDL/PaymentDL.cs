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
    }
}
