using CommonDataLayer.Enum;
using System.ComponentModel.DataAnnotations;

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

        public NotificationReceiveEnum ReceiveEnum { get; set; }

        public Guid? ResidentId { get; set; }

        public Guid? PaymentId { get; set; }

        [Required]
        public NotificationStatus Status { get; set; } = NotificationStatus.Read;
    }
}
