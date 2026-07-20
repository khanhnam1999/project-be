using BusinessLogicLayer;
using CommonDataLayer.DTO;
using CommonDataLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentsManagement.Ntier.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApartmentsController : BasesController<Apartment>
    {
        private readonly IApartmentBL _apartmentBL;

        public ApartmentsController(IApartmentBL apartmentBL) : base(apartmentBL)
        {
            _apartmentBL = apartmentBL;
        }

        [Authorize]
        [HttpGet("report")]
        public async Task<IActionResult> GetApartmentReport()
        {
            try
            {
                return Ok(await _apartmentBL.GetApartmentReport());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
