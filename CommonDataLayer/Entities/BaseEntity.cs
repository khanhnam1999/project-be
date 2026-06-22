using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommonDataLayer.Entities
{
    public class BaseEntity
    {
        [Required, DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        [Required, DataType(DataType.DateTime)]
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
    }
}
