using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommonDataLayer.Entities
{
    public class Service : BaseEntity
    {
        [Key]
        public Guid ServiceId { get; set; } = Guid.NewGuid();

        [Required, StringLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        // Giá thuê theo ngày
        [Range(0, 100000)]
        public decimal Price { get; set; }

        // Giá thuê theo tháng (có thể rẻ hơn so với Price * 30)
        [Range(0, 100000)]
        public decimal MonthlyPrice { get; set; }

        public ICollection<Booking>? Bookings { get; set; }
    }
}
