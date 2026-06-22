using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CommonDataLayer.Entities
{
    // Hợp đồng thuê hoặc mua căn hộ giữa cư dân và tòa nhà
    public class Contract : BaseEntity
    {
        [Key]
        public Guid ContractId { get; set; } = Guid.NewGuid();
        [ForeignKey("Resident")]
        public Guid ResidentId { get; set; }
        public Resident? Resident { get; set; }

        [ForeignKey("Apartment")]
        public Guid ApartmentId { get; set; }
        public Apartment? Apartment { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Required, StringLength(20)]
        public string Type { get; set; }
    }
}
