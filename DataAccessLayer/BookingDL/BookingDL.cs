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
            var query = _dbSet.Where(x => !x.IsDeleted && x.Status == BookingStatus.Using)
                .Include(x => x.Service)
                .Include(x => x.Apartment)
                .Include(x => x.Resident)
                    .ThenInclude(a => a.Account);

            return await query.ToListAsync();
        }
    }
}
