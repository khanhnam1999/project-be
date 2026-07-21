using CommonDataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer
{
    public interface IBookingDL : IBaseDL<Booking>
    {
        Task<List<Booking>> GetListBookingsUnPaid();
    }
}
