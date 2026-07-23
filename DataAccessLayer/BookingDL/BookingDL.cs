using CommonDataLayer.DTO;
using CommonDataLayer.Entities;
using CommonDataLayer.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer
{
    public class BookingDL : BaseDL<Booking>, IBookingDL
    {
        private readonly DbSet<Booking> _dbSet;
        private readonly CondoContext _condoContext;
        public BookingDL(CondoContext condoContext) : base(condoContext)
        {
            _condoContext = condoContext;
            _dbSet = condoContext.Set<Booking>();
        }

        public override FilterResult<Booking> FilterData(FilterData filterData)
        {
            var query = GetQueryFilterData(filterData);
            query = query.Include(x => x.Apartment)
                    .Include(x => x.Resident)
                    .ThenInclude(a => a.Account);
            FilterResult<Booking> result = new FilterResult<Booking>();
            result.TotalRecords = query.Count();
            result.Results = query.ToList();
            return result;
        }

        public async Task<List<Booking>> GetListBookingsUnPaid()
        {
            var query = _dbSet.Where(x =>
                    !x.IsDeleted &&
                    x.Status == BookingStatus.Using &&
                    x.Payment == null)
                .Include(x => x.Service)
                .Include(x => x.Apartment)
                .Include(x => x.Resident)
                    .ThenInclude(a => a.Account);

            return await query.ToListAsync();
        }

        public override Guid Update(Guid id, Booking entity)
        {
            if (id != entity.BookingId)
                throw new Exception("Không tìm thấy dữ liệu cần chỉnh sửa");

            var existing = _dbSet
                .Include(x => x.Service)
                .Include(x => x.Payment)
                .FirstOrDefault(x => x.BookingId == id && !x.IsDeleted);

            if (existing == null)
                throw new Exception("Không tìm thấy dữ liệu cần chỉnh sửa");

            var shouldCreatePayment =
                existing.Status != BookingStatus.Using &&
                entity.Status == BookingStatus.Using &&
                existing.Payment == null;

            if (entity.ResidentId != Guid.Empty)
                existing.ResidentId = entity.ResidentId;
            if (entity.ServiceId != Guid.Empty)
                existing.ServiceId = entity.ServiceId;
            if (entity.ApartmentId.HasValue)
                existing.ApartmentId = entity.ApartmentId;
            if (entity.StartDate != default)
                existing.StartDate = entity.StartDate;
            if (entity.EndDate != default)
                existing.EndDate = entity.EndDate;

            existing.Status = entity.Status;
            existing.BookingType = entity.BookingType;
            existing.ModifiedDate = DateTime.Now;

            if (shouldCreatePayment)
            {
                // Service may have changed in the request, so load the effective service.
                var service = existing.ServiceId == existing.Service?.ServiceId
                    ? existing.Service
                    : _condoContext.Set<Service>().Find(existing.ServiceId);

                if (service == null)
                    throw new Exception("Không tìm thấy dịch vụ của booking");

                _condoContext.Set<Payment>().Add(CreateServicePayment(existing, service));
            }

            _condoContext.SaveChanges();
            return id;
        }

        private static Payment CreateServicePayment(Booking booking, Service service)
        {
            var days = (booking.EndDate.Date - booking.StartDate.Date).Days + 1;
            if (days <= 0)
                throw new ArgumentException("Ngày kết thúc booking phải lớn hơn hoặc bằng ngày bắt đầu");

            decimal amount;
            if (booking.BookingType == BookingType.Monthly)
            {
                var daysInMonth = DateTime.DaysInMonth(booking.EndDate.Year, booking.EndDate.Month);
                amount = days <= daysInMonth
                    ? ((decimal)days / daysInMonth) * service.MonthlyPrice
                    : service.MonthlyPrice +
                      ((decimal)(days - daysInMonth) / daysInMonth) * service.MonthlyPrice;
            }
            else
            {
                amount = service.Price * days;
            }

            return new Payment
            {
                ResidentId = booking.ResidentId,
                BookingId = booking.BookingId,
                PaymentType = PaymentType.Service,
                PaymentStatus = PaymentStatus.Pending,
                Title = $"Đóng phí dịch vụ {service.Name}",
                Description = $"Phí dịch vụ {service.Name} từ {booking.StartDate:dd/MM/yyyy} đến {booking.EndDate:dd/MM/yyyy}",
                Amount = amount
            };
        }
    }
}
