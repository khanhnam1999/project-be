using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommonDataLayer.Entities
{
    public class Province : BaseEntity
    {
        [Key]
        public Guid ProvinceId { get; set; } = Guid.NewGuid();

        [Required]
        public string ProvinceName { get; set; }

        [Required]
        public string ProvinceNameEn { get; set; }

        public ICollection<Account>? Accounts { get; set; }
    }
}
