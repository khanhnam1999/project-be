using BusinessLogicLayer;
using CommonDataLayer.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentsManagement.Ntier.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProvincesController : BasesController<Province>
    {
        public ProvincesController(IBaseBL<Province> baseBL) : base(baseBL)
        {
        }
    }
}
