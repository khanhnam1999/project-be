using BusinessLogicLayer;
using CommonDataLayer.DTO;
using CommonDataLayer.Entities;
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
    }
}
