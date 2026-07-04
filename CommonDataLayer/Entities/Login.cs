using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommonDataLayer.Entities
{
    public class Login
    {
        [Required(ErrorMessage = "Số căn cước là bắt buộc")]
        [RegularExpression(@"^\d{12}$", ErrorMessage = "Căn cước chứa 12 số")]
        public string IdentityNumber { get; set; }

        [Required(ErrorMessage = "Password là bắt buộc")]
        public string Password { get; set; }
    }
}
