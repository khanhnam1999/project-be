using BusinessLogicLayer;
using CommonDataLayer.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentsManagement.Ntier.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncidentsController : BasesController<Incident>
    {
        private readonly IIncidentBL _incidentBL;
        public IncidentsController(IIncidentBL incidentBL) : base(incidentBL)
        {
            _incidentBL = incidentBL;
        }
    }
}
