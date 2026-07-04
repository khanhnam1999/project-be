using BusinessLogicLayer;
using CommonDataLayer.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentsManagement.Ntier.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WardsController : BasesController<Ward>
    {
        public WardsController(IBaseBL<Ward> baseBL) : base(baseBL)
        {
        }
    }
}
