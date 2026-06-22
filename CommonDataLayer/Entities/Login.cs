using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommonDataLayer.Entities
{
    public class Login
    {
        [Required(ErrorMessage = "Username là bắt buộc")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Username phải từ 3 đến 20 ký tự")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Username chỉ được chứa chữ, số và dấu gạch dưới")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password là bắt buộc")]
        public string Password { get; set; }
    }
}
