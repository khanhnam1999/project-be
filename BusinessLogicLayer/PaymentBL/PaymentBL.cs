

using CommonDataLayer.DTO;
using CommonDataLayer.Entities;
using CommonDataLayer.Enum;
using DataAccessLayer;
using Microsoft.Extensions.Configuration;

namespace BusinessLogicLayer
{
    public class PaymentBL : BaseBL<Payment>, IPaymentBL
    {
        private readonly IPaymentDL _paymentDL;
        private readonly IContractDL _contractDL;
        private readonly IBookingDL _bookingDL;
        private readonly IConfiguration _configuration;

        public PaymentBL(IPaymentDL paymentDL, IContractDL contractDL, IBookingDL bookingDL,
            IConfiguration configuration) : base(paymentDL)
        {
            _paymentDL = paymentDL;
            _contractDL = contractDL;
            _bookingDL = bookingDL;
            _configuration = configuration;
        }

        public async Task<List<Payment>> GenerateBookingPayments()
        {
            List<Payment> payments = new List<Payment>();
            
            await _paymentDL.AddPaymentsAsync(payments);

            return payments;
        }

        public async Task<List<Payment>> GeneratePayments()
        {
            List<Payment> payments = new List<Payment>();

            // Tạo danh sách hóa đơn đóng tiền thuê nhà
            List<Contract> contracts = await _contractDL.GetListContractsWithoutCashType();
            foreach (Contract contract in contracts)
            {
                Payment payment = new Payment();
                Apartment apartment = contract.Apartment;

                ContractResident contractResident = contract.ContractResidents.FirstOrDefault(x => x.ResidentType == ResidentTypeEnum.HomeOwner);

                payment.Title = $"Đóng tiền căn hộ {apartment.RoomNumber}";
                payment.ResidentId = contractResident.ResidentId;
                payment.ContractId = contractResident.ContractId;
                payment.Amount = apartment.RentPrice;
                payment.PaymentType = PaymentType.Rent;
                payment.Description = $"Bạn có lịch đóng tiền thuê căn hộ {apartment.RoomNumber} từ ngày hôm nay đến ngày mùng 10";
                payments.Add(payment);
            }

            // Tạo danh sách hóa đơn sử dụng dịch vụ
            List<Booking> bookings = await _bookingDL.GetListBookingsUnPaid();
            foreach (Booking booking in bookings)
            {
                Payment payment = new Payment();
                Service service = booking.Service;

                payment.Title = $"Đóng phí dịch vụ ${service.Name}";
                payment.ResidentId = booking.ResidentId;
                payment.BookingId = booking.BookingId;
                payment.PaymentType = PaymentType.Service;
                int days = (booking.EndDate - booking.StartDate).Days + 1;

                if (booking.BookingType == BookingType.Monthly)
                {
                    int daysInMonth = DateTime.DaysInMonth(booking.EndDate.Year, booking.EndDate.Month);
                    if (days < daysInMonth)
                    {
                        payment.Amount = ((decimal)days / daysInMonth) * service.MonthlyPrice;
                    }
                    else if (days == daysInMonth)
                    {
                        payment.Amount = service.MonthlyPrice;
                    }
                    else
                    {
                        int leftDays = days - daysInMonth;
                        payment.Amount = service.MonthlyPrice + ((decimal)leftDays / daysInMonth) * service.MonthlyPrice;
                    }
                }
                else
                {
                    payment.Amount = service.Price * days;
                }

                payments.Add(payment);
            }

            await _paymentDL.AddPaymentsAsync(payments);

            return payments;
        }

        public FilterResult<PaymentListDto> FilterPayments(FilterData filterData)
            => _paymentDL.FilterPayments(filterData);

        public Task<List<PaymentInvoiceDto>> GetMyInvoices(Guid accountId)
            => _paymentDL.GetInvoicesByAccountIdAsync(accountId);

        public async Task<CheckoutResponseDto> Checkout(Guid accountId, CheckoutRequestDto request)
        {
            if (request.PaymentMethod is not PaymentMethodEnum.Cash and not PaymentMethodEnum.BankTransfer)
                throw new ArgumentException("Chỉ hỗ trợ thanh toán tiền mặt hoặc chuyển khoản");

            var transactionId = Guid.NewGuid();
            var referenceCode = $"ECOHOME-{DateTime.UtcNow:yyMMdd}-{transactionId.ToString("N")[..8].ToUpperInvariant()}";
            var payments = await _paymentDL.CheckoutAsync(
                accountId, request.PaymentIds, request.PaymentMethod, transactionId, referenceCode);
            var status = request.PaymentMethod == PaymentMethodEnum.Cash
                ? PaymentStatus.AwaitingCashConfirmation
                : PaymentStatus.AwaitingBankTransfer;

            return new CheckoutResponseDto
            {
                TransactionId = transactionId,
                ReferenceCode = referenceCode,
                TotalAmount = payments.Sum(x => x.Amount),
                Status = status,
                BankTransfer = request.PaymentMethod == PaymentMethodEnum.BankTransfer
                    ? CreateBankTransferInfo(referenceCode, payments.Sum(x => x.Amount))
                    : null
            };
        }

        public Task<int> ConfirmTransaction(Guid transactionId)
            => _paymentDL.ConfirmTransactionAsync(transactionId);

        private BankTransferInfoDto CreateBankTransferInfo(string referenceCode, decimal amount)
        {
            var section = _configuration.GetSection("BankTransfer");
            var qrCodeUrl = section["QrCodeUrl"];
            if (!string.IsNullOrWhiteSpace(qrCodeUrl))
            {
                var separator = qrCodeUrl.Contains('?') ? "&" : "?";
                qrCodeUrl = $"{qrCodeUrl}{separator}amount={amount:0.##}&addInfo={Uri.EscapeDataString(referenceCode)}&accountName={Uri.EscapeDataString(section["AccountName"] ?? string.Empty)}";
            }

            return new BankTransferInfoDto
            {
                BankName = section["BankName"],
                AccountNumber = section["AccountNumber"],
                AccountName = section["AccountName"],
                TransferContent = referenceCode,
                QrCodeUrl = qrCodeUrl
            };
        }

        public async Task<List<PaymentReportDto>> GetReport(DateTime startDate, DateTime endDate, string periodType)
        {
            if (startDate > endDate) throw new Exception("Ngày bắt đầu không được lớn hơn ngày kết thúc");

            if (periodType == "monthly")
            {
                return await _paymentDL.GetReportByMonthly(startDate, endDate);
            }
            else if (periodType == "weekly")
            {
                return await _paymentDL.GetReportByWeekly(startDate, endDate);
            }
            else
            {
                throw new Exception("Chỉ lọc theo tháng hoặc tuần");
            }
        }
    }
}
