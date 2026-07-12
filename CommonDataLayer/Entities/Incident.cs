using CommonDataLayer.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CommonDataLayer.Entities
{
    // Sự cố liên quan đến căn hộ, ví dụ: hỏng hóc, mất điện, v.v.
    public class Incident : BaseEntity
    {
        [Key]
        public Guid IncidentId { get; set; } = Guid.NewGuid();
        [ForeignKey("Apartment")]
        public Guid ApartmentId { get; set; }
        public Apartment? Apartment { get; set; }

        [ForeignKey("Resident")]
        public Guid ReportedBy { get; set; }
        public Resident? Resident { get; set; }

        [Required]
        public string Description { get; set; }

        public IncidentStatusEnum Status { get; set; }
    }
}
