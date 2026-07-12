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

        [Required, DataType(DataType.Date)]
        public DateTime BookingDate { get; set; }

        public BookingStatus Status { get; set; }
    }
}
