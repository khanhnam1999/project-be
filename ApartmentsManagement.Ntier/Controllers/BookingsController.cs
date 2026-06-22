using BusinessLogicLayer;
using CommonDataLayer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentsManagement.Ntier.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : BasesController<Booking>
    {
        //private readonly IBookingBL _bookingBL;
        public BookingsController(IBaseBL<Booking> bookingBL) : base(bookingBL)
        {
            //_bookingBL = bookingBL;
        }
    }
}
