using CommonDataLayer.Enum;
using System.ComponentModel.DataAnnotations;

namespace CommonDataLayer.Entities
{
    public class Apartment : BaseEntity
    {
        [Key]
        public Guid ApartmentId { get; set; } = Guid.NewGuid();

        // Tầng
        [Range(1, 100)]
        public int Floor { get; set; }

        // Số phòng
        [Required, StringLength(10)]
        public string RoomNumber { get; set; }

        // Diện tích căn phòng
        [Range(10, 1000)]
        public decimal Area { get; set; }

        // Trạng thái căn hộ (ví dụ: “Available = 0”, “Occupied = 1”, “Maintenance = 2”).
        [Required, Range(0, 2)]
        public ApartmentStatusEnum Status { get; set; }

        // Ảnh căn hộ
        public string? PictureUrl { get; set; }

        // Giá thuê
        [Required]
        public decimal RentPrice { get; set; }

        // Giá mua
        [Required]
        public decimal BuyPrice { get; set; }

        public ICollection<Contract>? Contracts { get; set; }
        public ICollection<Incident>? Incidents { get; set; }
        public ICollection<Booking>? Bookings { get; set; }
    }
}
