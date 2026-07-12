using CommonDataLayer.DTO;
using CommonDataLayer.Entities;
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
            query = query.Include(x => x.Resident)
                    .ThenInclude(a => a.Account);
            FilterResult<Booking> result = new FilterResult<Booking>();
            result.TotalRecords = query.Count();
            result.Results = query.ToList();
            return result;
        }
    }
}
