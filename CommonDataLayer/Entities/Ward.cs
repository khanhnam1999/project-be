using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommonDataLayer.Entities
{
    public class Ward : BaseEntity
    {
        [Key]
        public Guid WardId { get; set; } = Guid.NewGuid();

        [Required]
        public string WardName { get; set; }

        [Required]
        public string WardNameEn { get; set; }

        [ForeignKey("Province")]
        public Guid ProvinceId {  get; set; }

        public Province? Province { get; set; }

        public ICollection<Account>? Accounts { get; set; }
    }
}
