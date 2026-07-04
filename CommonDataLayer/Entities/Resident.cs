using CommonDataLayer.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;
using System.Text;

namespace CommonDataLayer.Entities
{
    // Cư dân sống trong căn hộ, có thể là người thuê hoặc chủ sở hữu
    public class Resident : BaseEntity
    {
        [Key]
        public Guid ResidentId { get; set; } = Guid.NewGuid();

        [Required, ForeignKey("Account")]
        public Guid AccountId { get; set; }
        public Account? Account { get; set; }

        // Danh sách hợp đồng theo cư dân (nhiều - nhiều)
        public ICollection<ContractResident>? ContractResidents { get; set; }

        // Danh sách các khoản thanh toán mà cư dân đã thực hiện
        public ICollection<Payment>? Payments { get; set; }

        // Danh sách các đặt chỗ (nếu cư dân có thể đặt các dịch vụ hoặc tiện ích trong tòa nhà)
        public ICollection<Booking>? Bookings { get; set; }

        // Danh sách các sự cố mà cư dân đã báo cáo
        public ICollection<Incident>? Incidents { get; set; }

    }
}
