using BusinessLogicLayer;
using CommonDataLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentsManagement.Ntier.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractsController : BasesController<Contract>
    {
        private readonly IContractBL _contractBL;
        private readonly IBaseBL<ContractResident> _contractResidentBL;
        public ContractsController(IContractBL contractBL, IBaseBL<ContractResident> contractResidentBL) : base(contractBL)
        {
            _contractBL = contractBL;
            _contractResidentBL = contractResidentBL;
        }

        [Authorize]
        [HttpPost("addResidentToContract")]
        public IActionResult AddResidentToContract([FromBody] ContractResident contractResident)
        {
            try
            {
                Guid result = _contractResidentBL.Add(contractResident);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [Authorize]
        [HttpPost("updateResidentToContract")]
        public IActionResult RemoveResidentToContract([FromBody] ContractResident contractResident)
        {
            try
            {
                Guid result = _contractBL.UpdateResidentToContract(contractResident);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
