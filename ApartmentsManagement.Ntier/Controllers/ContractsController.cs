using BusinessLogicLayer;
using CommonDataLayer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentsManagement.Ntier.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractsController : BasesController<Contract>
    {
        public ContractsController(IBaseBL<Contract> baseBL) : base(baseBL)
        {
        }
    }
}
