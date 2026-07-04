namespace CommonDataLayer.Enum
{
    // Trạng thái của căn hộ, ví dụ: 0 - Available (Có sẵn), 1 - Occupied (Đã thuê), 2 - Maintenance (Bảo trì)
    public enum ApartmentStatusEnum : byte
    {
        Available = 0,
        Occupied = 1,
        Maintenance = 2
    }

    // Vai trò của người dùng trong hệ thống, ví dụ: 0 - Resident (Cư dân), 1 - Owner (Chủ sở hữu), 2 - Staff (Nhân viên)
    public enum RoleEnum : byte
    {
        Resident = 0,
        Onwer = 1,
        Staff = 2,
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

    // Trạng thái của hợp đồng, ví dụ: 0 - Cash (Mua trả thẳng, 1 - Installment (Mua trả góp), 2 - Rental (Thuê)
    public enum ContractTypeEnum : byte
    {
        Cash = 0,
        Installment = 1,
        Rental = 2,
    }

    // Phương thức thanh toán, ví dụ: 0 - Cash (Tiền mặt), 1 - BankTransfer (Chuyển khoản), 2 - CreditCard (Thẻ tín dụng), 3 - EWallet(Ví thanh toán)
    public enum PaymentMethodEnum : byte
    {
        Cash = 0,
        BankTransfer = 1,
        CreditCard = 2,
        EWallet = 3
    }

    // Thông tin của thông báo: 0 - Read (Đã đọc), 1 - Unread (Chưa đọc)
    public enum NotificationStatus : byte
    {
        Read = 0,
        Unread = 1,
    }

    // Thông tin giới tính người dùng: 0 - Male (Nam), 1 - Female (Nữ), 2 - Other (Khác)
    public enum GenderEnum : byte
    {
        Male = 0,
        Female = 1,
        Other = 2,
    }

    // Thông tin gửi cho ai: 0 - Tất cả, 1 - Dân cư, 2 - Nhân viên
    public enum NotificationReceiveEnum :byte
    {
        All = 0,
        Resident = 1,
        Staff = 2,
    }
}
