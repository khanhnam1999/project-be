using BusinessLogicLayer;
using CommonDataLayer.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentsManagement.Ntier.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncidentsController : BasesController<Incident>
    {
        public IncidentsController(IBaseBL<Incident> baseBL) : base(baseBL)
        {
        }
    }
}
