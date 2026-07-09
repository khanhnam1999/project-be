using BusinessLogicLayer;
using CommonDataLayer.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentsManagement.Ntier.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResidentsController : BasesController<Resident>
    {
        private readonly IResidentBL _residentBL;
        public ResidentsController(IResidentBL residentBL) : base(residentBL)
        {
            _residentBL = residentBL;
        }
    }
}
