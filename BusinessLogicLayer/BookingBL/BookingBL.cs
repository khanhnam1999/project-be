using CommonDataLayer.Entities;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicLayer
{
    public class BookingBL : BaseBL<Booking>, IBookingBL
    {
        private readonly IBookingDL _bookingDL;
        public BookingBL(IBookingDL bookingDL) : base(bookingDL)
        {
            _bookingDL = bookingDL;
        }
    }
}
