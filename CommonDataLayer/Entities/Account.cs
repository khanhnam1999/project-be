using CommonDataLayer.Enum;
using System.ComponentModel.DataAnnotations;

namespace CommonDataLayer.Entities
{
    public class Account : BaseEntity
    {
        [Key]
        public Guid AccountId { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Tên là bắt buộc"), StringLength(100)]
        public string FullName { get; set; }

        [Phone]
        [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email là bắt buộc")]
        public string Email { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Ngày sinh là bắt buộc")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Username là bắt buộc")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Username phải từ 3 đến 20 ký tự")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Username chỉ được chứa chữ, số và dấu gạch dưới")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password là bắt buộc")]
        public string Password { get; set; }

        public string? Token { get; set; }

        [Required(ErrorMessage = "Role là bắt buộc")]
        public RoleEnum Role { get; set; } = RoleEnum.People;

        public Resident? Resident { get; set; }
    }
}
