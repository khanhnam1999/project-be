namespace CommonDataLayer.Enum
{
    // Trạng thái của căn hộ, ví dụ: 0 - Available (Có sẵn), 1 - Occupied (Đã có người ở), 2 - Maintenance (Bảo trì)
    public enum ApartmentStatusEnum : byte
    {
        Available = 0,
        Pending = 1,
        Occupied = 2,
        Maintenance = 3
    }

    // Vai trò của người dùng trong hệ thống, ví dụ: 0 - Resident (Cư dân), 1 - Owner (Chủ sở hữu), 2 - Staff (Nhân viên)
    public enum RoleEnum : byte
    {
        Resident = 0,
        Onwer = 1,
        Staff = 2,
    }

    // Loại cư dân trong căn hộ, ví dụ: 0 - HomeOwner (Chủ hộ), 1 - Spouse (Vợ/Chồng), 2 - Child (Con cái), 3 - Tenant (Người thuê)
    public enum ResidentTypeEnum : byte
    {
        HomeOwner = 0,
        Spouse = 1,
        Child = 2,
        Tenant = 3
    }

    // Trạng thái của hợp đồng, ví dụ: 0 - Cash (Mua trả thẳng, 1 - Rental (Thuê)
    public enum ContractTypeEnum : byte
    {
        Cash = 0,
        Rental = 1,
    }

    // Phương thức thanh toán, ví dụ: 0 - Cash (Tiền mặt), 1 - BankTransfer (Chuyển khoản), 2 - CreditCard (Thẻ tín dụng), 3 - EWallet(Ví thanh toán)
    public enum PaymentMethodEnum : byte
    {
        Cash = 0,
        BankTransfer = 1,
        CreditCard = 2,
        EWallet = 3,
    }

    // Trạng thái thanh toán
    public enum PaymentStatus : byte
    {
        Pending = 0,
        AwaitingBankTransfer = 1,
        AwaitingCashConfirmation = 2,
        Paid = 3,
        Overdue = 4,
        Cancelled = 5
    }

    public enum PaymentType : byte
    {
        Rent = 0,
        Service = 1
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
    public enum NotificationReceiveEnum : byte
    {
        All = 0,
        Resident = 1,
        Staff = 2,
    }

    // Thông tin trạng thái đặt dịch vụ: New: tạo mới, Using: Đang sử dụng dịch vụ , Closed: Đã sử dụng dịch vụ xong
    public enum BookingStatus : byte
    {
        New,        
        Using,
        Closed,
    }

    public enum BookingType : byte
    {
        Daily,      // Book theo ngày
        Monthly,    // Book theo tháng
    }

    // Thông tin trạng thái sự cố
    public enum IncidentStatusEnum : byte
    {
        New = 0,            // Tạo mới
        InProgress = 1,     // Đang điều tra
        Resolved = 2,       // đã có giải pháp, sự cố được khắc phục
        Closed = 3,         // sự cố đã hoàn tất, không cần hành động thêm
        Reopened = 4,       // sự cố đã đóng nhưng phát sinh lại
        Cancelled = 5       // sự cố bị hủy, không xử lý nữa.
    }
}
