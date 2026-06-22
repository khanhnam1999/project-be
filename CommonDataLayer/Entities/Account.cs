using CommonDataLayer.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CommonDataLayer.Entities
{
    public class Account : BaseEntity
    {
        [Key]
        public Guid AccountId { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Tên là bắt buộc"), StringLength(100)]
        public string FullName { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Username là bắt buộc")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Username phải từ 3 đến 20 ký tự")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Username chỉ được chứa chữ, số và dấu gạch dưới")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password là bắt buộc")]
        public string Password { get; set; }

        public string? Token { get; set; }

        public RoleEnum Role { get; set; } = RoleEnum.People;
        public Resident? Resident { get; set; }
    }
}
