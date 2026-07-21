using CommonDataLayer.Enum;
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
        
        [ForeignKey("Apartment")]
        public Guid ApartmentId { get; set; }
        public Apartment? Apartment { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        // Danh sách cư dân theo hợp đồng ( nhiều - nhiều)
        public ICollection<ContractResident>? ContractResidents { get; set; }

        // Trạng thái của hợp đồng, ví dụ: 0 - Cash (Mua trả thẳng, 1 - Rental (Thuê)
        [Required]
        public ContractTypeEnum Type { get; set; }

        // Danh sách hóa đơn
        public ICollection<Payment>? Payments { get; set; }
    }
}
