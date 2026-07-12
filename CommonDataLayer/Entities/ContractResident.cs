using CommonDataLayer.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommonDataLayer.Entities
{
    public class ContractResident : BaseEntity
    {
        [Key]
        [ForeignKey("Contract")]
        public Guid ContractId { get; set; }

        public Contract? Contract { get; set; }

        [Key]
        [ForeignKey("Resident")]
        public Guid ResidentId { get; set; }

        public Resident? Resident { get; set; }

        // Loại cư dân: 1 - Vợ/Chồng, 2 - Con cái, 3 - Người thuê, 0 - Chủ hộ
        [Required(ErrorMessage = "ResidentType is required")]
        public ResidentTypeEnum ResidentType { get; set; }
    }
}
