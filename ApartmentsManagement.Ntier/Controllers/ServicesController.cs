using BusinessLogicLayer;
using CommonDataLayer.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentsManagement.Ntier.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : BasesController<Service>
    {
        private readonly IServiceBL _serviceBL;

        public ServicesController(IServiceBL serviceBL) : base(serviceBL) => _serviceBL = serviceBL;
    }
}
