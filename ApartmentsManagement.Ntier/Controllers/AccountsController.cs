using BusinessLogicLayer;
using CommonDataLayer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            catch (DbUpdateException ex)
            {
                // Kiểm tra xem lỗi có phải do SQL Server ném về không
                if (ex.InnerException is Microsoft.Data.SqlClient.SqlException sqlEx)
                {
                    // Mã 2601 hoặc 2627 là lỗi trùng Unique Index
                    if (sqlEx.Number == 2601 || sqlEx.Number == 2627)
                    {
                        // 📝 Đây chính là nơi bạn tạo ra "Message" tùy chỉnh của mình:
                        return BadRequest(new { message = "Số điện thoại này đã được sử dụng trên hệ thống. Vui lòng nhập số khác!" });
                    }
                }

                // Các lỗi database khác nếu có
                return StatusCode(500, new { message = "Có lỗi xảy ra trong quá trình lưu dữ liệu." });
            }
        }
    }
}
