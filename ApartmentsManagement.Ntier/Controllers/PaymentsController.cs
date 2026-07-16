using BusinessLogicLayer;
using CommonDataLayer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace ApartmentsManagement.Ntier.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : BasesController<Payment>
    {
        private readonly IPaymentBL _paymentBL;
        public PaymentsController(IPaymentBL paymentBL) : base(paymentBL)
        {

        }
    }
}
