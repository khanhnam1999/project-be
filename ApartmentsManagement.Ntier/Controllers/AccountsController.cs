using BusinessLogicLayer;
using CommonDataLayer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentsManagement.Ntier.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : BasesController<Account>
    {
        private readonly IAccountBL _accountBL;

        public AccountsController(IAccountBL accountBL) : base(accountBL)
        {
            _accountBL = accountBL;
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate(Login login)
        {
            try
            {
                Account account = _accountBL.Authenticate(login);
                return Ok(account);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] Account account) {
            try
            {
                Guid id = _accountBL.Register(account);
                return Ok(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
