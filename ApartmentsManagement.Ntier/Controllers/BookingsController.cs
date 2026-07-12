using BusinessLogicLayer;
using CommonDataLayer.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentsManagement.Ntier.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : BasesController<Booking>
    {
        private readonly IBookingBL _bookingBL;
        public BookingsController(IBookingBL bookingBL) : base(bookingBL)
        {
            _bookingBL = bookingBL;
        }
    }
}
