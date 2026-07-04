using BusinessLogicLayer;
using CommonDataLayer.DTO;
using CommonDataLayer.Entities;
using CommonDataLayer.Untilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace ApartmentsManagement.Ntier.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : BasesController<Account>
    {
        private readonly IAccountBL _accountBL;
        private readonly SmsService _smsService;

        public AccountsController(IAccountBL accountBL, SmsService smsService) : base(accountBL)
        {
            _accountBL = accountBL;
            _smsService = smsService;
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
            catch (DbUpdateException ex)
            {
                // Kiểm tra xem lỗi có phải do SQL Server ném về không
                if (ex.InnerException is Microsoft.Data.SqlClient.SqlException sqlEx)
                {
                    // Mã 2601 hoặc 2627 là lỗi trùng Unique Index
                    if (sqlEx.Number == 2601 || sqlEx.Number == 2627)
                    {
                        // 📝 Đây chính là nơi bạn tạo ra "Message" tùy chỉnh của mình:
                        return BadRequest(new { message = "This identity number is exists!" });
                    }
                }

                // Các lỗi database khác nếu có
                return StatusCode(500, new { message = "Có lỗi xảy ra trong quá trình lưu dữ liệu." });
            }
        }

        [HttpGet("checkAccount/{identityNumber}")]
        public IActionResult CheckAccount([FromRoute] string identityNumber)
        {
            try
            {
                string email = _accountBL.GetEmail(identityNumber);

                return Ok(email);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("requestOtp/{email}")]
        public IActionResult RequestOtp([FromRoute] string email)
        {
            try
            {
                _smsService.GenerateAndSaveOtp(email);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("verifyOtp")]
        public IActionResult VerifyOtp([FromBody] VerifyOtpRequest verifyOtpRequest)
        {
            try
            {
                bool result = _smsService.VerifyOtp(verifyOtpRequest.Email, verifyOtpRequest.Otp);
                if(result)
                {
                    return Ok();
                }
                else
                {
                    return StatusCode(400, "Invalid OTP");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("setPwd")]
        public IActionResult SetPwd([FromBody] SetPwdData setPwdData) {
            try
            {
                Guid id =  _accountBL.SetPwd(setPwdData.IdentityNumber, setPwdData.Email, setPwdData.Password);

                return Ok(id);    
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
