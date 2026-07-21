using CommonDataLayer.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CommonDataLayer.Entities
{
    public class Booking : BaseEntity
    {
        [Key]
        public Guid BookingId { get; set; } = Guid.NewGuid();

        [ForeignKey("Resident")]
        public Guid ResidentId { get; set; }
        public Resident? Resident { get; set; }

        [ForeignKey("Service")]
        public Guid ServiceId { get; set; }
        public Service? Service { get; set; }

        [ForeignKey("Apartment")]
        public Guid? ApartmentId { get; set; }
        public Apartment? Apartment { get; set; }

        [Required, DataType(DataType.Date)]
        // Khoảng thời gian đặt (theo tháng)
        public DateTime StartDate { get; set; }   // ví dụ: 2026-07-01

        [Required, DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public BookingStatus Status { get; set; } = BookingStatus.New;

        public BookingType BookingType { get; set; } = BookingType.Daily;

        public Payment? Payment { get; set; }
    }
}
