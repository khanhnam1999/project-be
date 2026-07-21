using CommonDataLayer.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CommonDataLayer.Entities
{
    // Thông tin thanh toán, ví dụ: tiền thuê, phí dịch vụ, v.v.
    public class Payment : BaseEntity
    {
        [Key]
        public Guid PaymentId { get; set; } = Guid.NewGuid();

        [ForeignKey("Resident")]
        public Guid ResidentId { get; set; }
        public Resident? Resident { get; set; }

        public Guid? ContractId { get; set; }
        public Contract? Contract { get; set; }

        public Guid? BookingId { get; set; }
        public Booking? Booking { get; set; }

        [Required]
        public string Title { get; set; }

        public string? Description { get; set; }

        public decimal Amount { get; set; }

        public DateTime? PaymentDate { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime PaymentDeadline { get; set; } = DateTime.Now.AddDays(10);

        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

        public PaymentType PaymentType { get; set; }

        public PaymentMethodEnum? PaymentMethod { get; set; }

        public Guid? TransactionId { get; set; }

        [StringLength(50)]
        public string? ReferenceCode { get; set; }
    }
}
