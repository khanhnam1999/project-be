using CommonDataLayer.DTO;
using CommonDataLayer.Entities;
using CommonDataLayer.Enum;
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
            await _dbSet.AddRangeAsync(payments);
            await _condoContext.SaveChangesAsync();
        }

        public FilterResult<PaymentListDto> FilterPayments(FilterData filterData)
        {
            var query = _dbSet
                .AsNoTracking()
                .Where(p => !p.IsDeleted);

            query = ApplyOrdering(query, filterData.SortName, filterData.SortMethod);

            foreach (var condition in filterData.Conditions ?? [])
            {
                if (condition.Key == nameof(Payment.PaymentMethod))
                {
                    query = query.Where(p => p.PaymentMethod == condition.PaymentStatusValue);
                }
                else if (condition.GuidValue.HasValue)
                {
                    query = query.Where(CreateLambda(condition.Key, condition.GuidValue.Value));
                }
                else if (condition.Value != null)
                {
                    query = query.Where(CreateLambda(condition.Key, condition.Value, "Contains"));
                }
                else
                {
                    throw new ArgumentException($"Điều kiện {condition.Key} không có giá trị");
                }
            }

            var totalRecords = query.Count();

            if (filterData.Page > 0)
            {
                query = query
                    .Skip((filterData.Page - 1) * filterData.Limit)
                    .Take(filterData.Limit);
            }

            var results = query
                .Select(p => new PaymentListDto
                {
                    PaymentId = p.PaymentId,
                    ResidentId = p.ResidentId,
                    ContractId = p.ContractId,
                    BookingId = p.BookingId,
                    Title = p.Title,
                    Description = p.Description,
                    Amount = p.Amount,
                    PaymentDate = p.PaymentDate,
                    PaymentDeadline = p.PaymentDeadline,
                    PaymentStatus = p.PaymentStatus,
                    PaymentType = p.PaymentType,
                    PaymentMethod = p.PaymentMethod,
                    TransactionId = p.TransactionId,
                    ReferenceCode = p.ReferenceCode,
                    ResidentName = p.Resident != null && p.Resident.Account != null
                        ? p.Resident.Account.FullName
                        : null,
                    RoomNumber = p.Contract != null && p.Contract.Apartment != null
                        ? p.Contract.Apartment.RoomNumber
                        : p.Booking != null && p.Booking.Apartment != null
                            ? p.Booking.Apartment.RoomNumber
                            : null
                })
                .ToList();

            return new FilterResult<PaymentListDto>
            {
                TotalRecords = totalRecords,
                Results = results
            };
        }

        public async Task<List<PaymentInvoiceDto>> GetInvoicesByAccountIdAsync(Guid accountId)
        {
            var now = DateTime.Now;
            var payments = await _dbSet
                .Where(p => !p.IsDeleted && p.Resident.AccountId == accountId)
                .Include(p => p.Contract).ThenInclude(c => c.Apartment)
                .Include(p => p.Booking).ThenInclude(b => b.Apartment)
                .OrderBy(p => p.PaymentDeadline)
                .ToListAsync();

            foreach (var payment in payments.Where(p => p.PaymentStatus == PaymentStatus.Pending && p.PaymentDeadline < now))
                payment.PaymentStatus = PaymentStatus.Overdue;

            if (_condoContext.ChangeTracker.HasChanges())
                await _condoContext.SaveChangesAsync();

            return payments.Select(p => new PaymentInvoiceDto
            {
                PaymentId = p.PaymentId,
                Title = p.Title,
                Description = p.Description,
                RoomNumber = p.Contract?.Apartment?.RoomNumber ?? p.Booking?.Apartment?.RoomNumber,
                Amount = p.Amount,
                Deadline = p.PaymentDeadline,
                Type = p.PaymentType,
                Status = p.PaymentStatus,
                PaymentDate = p.PaymentDate
            }).ToList();
        }

        public async Task<int> ConfirmTransactionAsync(Guid transactionId)
        {
            var payments = await _dbSet
                .Where(p => p.TransactionId == transactionId && !p.IsDeleted)
                .ToListAsync();

            if (payments.Count == 0)
                throw new KeyNotFoundException("Không tìm thấy giao dịch");
            if (payments.All(p => p.PaymentStatus == PaymentStatus.Paid))
                return payments.Count;
            if (payments.Any(p => p.PaymentStatus is not PaymentStatus.AwaitingBankTransfer
                                  and not PaymentStatus.AwaitingCashConfirmation
                                  and not PaymentStatus.Paid))
                throw new InvalidOperationException("Giao dịch không ở trạng thái chờ xác nhận");

            var paidAt = DateTime.Now;
            foreach (var payment in payments.Where(p => p.PaymentStatus != PaymentStatus.Paid))
            {
                payment.PaymentStatus = PaymentStatus.Paid;
                payment.PaymentDate = paidAt;
                payment.ModifiedDate = paidAt;
            }

            await _condoContext.SaveChangesAsync();
            return payments.Count;
        }

        public async Task<List<Payment>> CheckoutAsync(Guid accountId, IReadOnlyCollection<Guid> paymentIds,
            PaymentMethodEnum paymentMethod, Guid transactionId, string referenceCode)
        {
            var uniqueIds = paymentIds.Distinct().ToList();
            if (uniqueIds.Count == 0)
                throw new ArgumentException("Phải chọn ít nhất một hóa đơn");

            var payments = await _dbSet
                .Where(p => uniqueIds.Contains(p.PaymentId) && p.Resident.AccountId == accountId && !p.IsDeleted)
                .ToListAsync();

            if (payments.Count != uniqueIds.Count)
                throw new KeyNotFoundException("Có hóa đơn không tồn tại hoặc không thuộc cư dân hiện tại");
            if (payments.Any(p => p.PaymentStatus is PaymentStatus.Paid or PaymentStatus.Cancelled
                                  or PaymentStatus.AwaitingBankTransfer or PaymentStatus.AwaitingCashConfirmation))
                throw new InvalidOperationException("Chỉ hóa đơn đang chờ hoặc quá hạn mới có thể checkout");

            var status = paymentMethod == PaymentMethodEnum.Cash
                ? PaymentStatus.AwaitingCashConfirmation
                : PaymentStatus.AwaitingBankTransfer;
            foreach (var payment in payments)
            {
                payment.PaymentMethod = paymentMethod;
                payment.PaymentStatus = status;
                payment.TransactionId = transactionId;
                payment.ReferenceCode = referenceCode;
                payment.ModifiedDate = DateTime.Now;
            }

            await _condoContext.SaveChangesAsync();
            return payments;
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
