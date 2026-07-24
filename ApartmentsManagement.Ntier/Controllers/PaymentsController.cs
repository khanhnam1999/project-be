using BusinessLogicLayer;
using CommonDataLayer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authorization;
using CommonDataLayer.DTO;
using System.Security.Claims;

namespace ApartmentsManagement.Ntier.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentBL _paymentBL;
        public PaymentsController(IPaymentBL paymentBL)
        {
            _paymentBL = paymentBL;
        }

        [Authorize(Roles = "Owner,Onwer,Staff")]
        [HttpGet]
        public IActionResult GetAll() => Ok(_paymentBL.GetAll());

        [Authorize(Roles = "Owner,Onwer,Staff")]
        [HttpGet("{id:guid}")]
        public IActionResult GetById(Guid id) => Ok(_paymentBL.GetById(id));

        [Authorize(Roles = "Owner,Onwer,Staff")]
        [HttpPost("filter")]
        public IActionResult Filter([FromBody] FilterData filterData) => Ok(_paymentBL.FilterPayments(filterData));

        [Authorize(Roles = "Owner,Onwer,Staff")]
        [HttpPost]
        public IActionResult Add([FromBody] Payment payment)
            => CreatedAtAction(nameof(GetById), new { id = _paymentBL.Add(payment) }, null);

        [Authorize(Roles = "Owner,Onwer,Staff")]
        [HttpPut("{id:guid}")]
        public IActionResult Update(Guid id, [FromBody] Payment payment) => Ok(_paymentBL.Update(id, payment));

        [Authorize(Roles = "Owner,Onwer,Staff")]
        [HttpPost("delete")]
        public IActionResult Delete([FromBody] List<Guid> ids) => Ok(_paymentBL.Delete(ids));

        [Authorize(Roles = "Owner,Onwer,Staff")]
        [HttpPost("restore")]
        public IActionResult Restore([FromBody] List<Guid> ids) => Ok(_paymentBL.Restore(ids));

        [Authorize(Roles = "Owner,Onwer,Staff")]
        [HttpPost("delete-hard")]
        public IActionResult DeleteHard([FromBody] List<Guid> ids) => Ok(_paymentBL.DeleteHard(ids));

        [Authorize(Roles = "Resident")]
        [HttpGet("my-invoices")]
        public async Task<IActionResult> GetMyInvoices()
        {
            if (!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var accountId))
                return Unauthorized("Token không chứa định danh tài khoản");

            return Ok(await _paymentBL.GetMyInvoices(accountId));
        }

        [Authorize(Roles = "Resident")]
        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromBody] CheckoutRequestDto request)
        {
            if (!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var accountId))
                return Unauthorized("Token không chứa định danh tài khoản");

            try
            {
                return Ok(await _paymentBL.Checkout(accountId, request));
            }
            catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
            catch (InvalidOperationException ex) { return Conflict(ex.Message); }
        }

        [Authorize(Roles = "Owner,Onwer,Staff")]
        [HttpPut("transactions/{transactionId:guid}/confirm")]
        public async Task<IActionResult> ConfirmTransaction(Guid transactionId)
        {
            try
            {
                return Ok(new { UpdatedPayments = await _paymentBL.ConfirmTransaction(transactionId) });
            }
            catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
            catch (InvalidOperationException ex) { return Conflict(ex.Message); }
        }

        [Authorize(Roles = "Owner,Onwer,Staff")]
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

        //[HttpPut("paid")]
        //public async Task
    }
}
