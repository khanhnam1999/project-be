using System;
using System.Collections.Generic;
using System.Text;

namespace CommonDataLayer.Enum
{
    // Trạng thái của căn hộ, ví dụ: 0 - Available (Có sẵn), 1 - Occupied (Đã thuê), 2 - Maintenance (Bảo trì)
    public enum ApartmentStatusEnum : byte
    {
        Available = 0,
        Occupied = 1,
        Maintenance = 2
    }

    // Vai trò của người dùng trong hệ thống, ví dụ: 0 - People (Người dùng thông thường), 1 - Owner (Chủ sở hữu), 2 - Staff (Nhân viên), 3 - Resident (Cư dân)
    public enum RoleEnum : byte
    {
        People = 0,
        Onwer = 1,
        Staff = 2,
        Resident = 3,
    }

    // Loại cư dân trong căn hộ, ví dụ: 3 - HomeOwner (Chủ hộ), 0 - Spouse (Vợ/Chồng), 1 - Child (Con cái), 2 - Tenant (Người thuê)
    public enum ResidentTypeEnum : byte
    {
        HomeOwner = 0,
        Spouse = 1,
        Child = 2,
        Tenant = 3
    }

    // Trạng thái của sự cố, ví dụ: 0 - Open (Mở), 1 - In Progress (Đang xử lý), 2 - Resolved (Đã giải quyết), 3 - Closed (Đã đóng)
    public enum IncidentStatusEnum : byte
    {
        Open = 0,
        InProgress = 1,
        Resolved = 2,
        Closed = 3
    }
}
