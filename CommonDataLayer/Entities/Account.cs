using CommonDataLayer.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [Required(ErrorMessage = "Giới tính là bắt buộc")]
        public GenderEnum Gender { get; set; }

        [Required(ErrorMessage = "Số căn cước là bắt buộc")]
        [RegularExpression(@"^\d{12}$", ErrorMessage = "Căn cước chứa 12 số")]
        public string IdentityNumber { get; set; }

        [Required(ErrorMessage = "Ngày cấp căn cước là bắt buộc")]
        public DateTime IdentityIssuedDate { get; set; }

        [Required(ErrorMessage = "Nơi cấp căn cước là bắt buộc")]
        public string IdentityIssuedPlace { get; set; }

        [ForeignKey("Province")]
        public Guid ProvinceId { get; set; }

        public Province? Province { get; set; }

        [ForeignKey("Ward")]
        public Guid WardId { get; set; }

        public Ward? Ward { get; set; }

        [Required(ErrorMessage = "Địa chỉ là bắt buộc")]
        public string AddressDetail { get; set; }

        //[Required(ErrorMessage = "Username là bắt buộc")]
        //[StringLength(20, MinimumLength = 3, ErrorMessage = "Username phải từ 3 đến 20 ký tự")]
        //[RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Username chỉ được chứa chữ, số và dấu gạch dưới")]
        //public string Username { get; set; }

        public string? Password { get; set; }

        public string? Token { get; set; }

        [Required(ErrorMessage = "Role là bắt buộc")]
        public RoleEnum Role { get; set; } = RoleEnum.Resident;
    }
}
