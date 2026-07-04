using CommonDataLayer.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommonDataLayer.Entities
{
    public class ContractResident : BaseEntity
    {
        [ForeignKey("Contract")]
        public Guid ContractId { get; set; }

        public Contract? Contract { get; set; }

        [ForeignKey("Resident")]
        public Guid ResidentId { get; set; }

        public Resident? Resident { get; set; }

        // Loại cư dân: 0 - Vợ/Chồng, 1 - Con cái, 2 - Người thuê, 3 - Chủ hộ
        [Required(ErrorMessage = "ResidentType is required")]
        public ResidentTypeEnum ResidentType { get; set; }
    }
}
