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

        [Required]
        public string Title { get; set; }

        public string? Description { get; set; }

        [Range(0, 100000000)]
        public decimal Amount { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime? PaymentDate { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime PaymentDeadline { get; set; }

        public PaymentMethodEnum? PaymentMethod { get; set; }
    }
}
