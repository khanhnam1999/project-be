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
            _paymentBL = paymentBL;
        }

        [HttpGet("report")]
        public async Task<IActionResult> GetReport(DateTime startDate, DateTime endDate, string periodType = "month")
        {
            try
            {
                var results = await _paymentBL.GetReport(startDate, endDate, periodType);

                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
