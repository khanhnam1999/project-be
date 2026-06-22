using BusinessLogicLayer;
using CommonDataLayer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentsManagement.Ntier.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResidentsController : BasesController<Resident>
    {
        public ResidentsController(IBaseBL<Resident> baseBL) : base(baseBL)
        {
        }
    }
}
