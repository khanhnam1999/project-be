using CommonDataLayer.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommonDataLayer.Entities
{
    public class Notification : BaseEntity
    {
        [Key]
        public Guid NotificationId { get; set; } = Guid.NewGuid();

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }
        
        [Required]
        public Guid ResidentId { get; set; }

        public Guid? PaymentId { get; set; }

        [Required]
        public NotificationStatus Status { get; set; } = NotificationStatus.Read;
    }
}
