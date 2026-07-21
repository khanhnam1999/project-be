

using CommonDataLayer.DTO;
using CommonDataLayer.Entities;
using CommonDataLayer.Enum;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public class PaymentBL : BaseBL<Payment>, IPaymentBL
    {
        private readonly IPaymentDL _paymentDL;
        private readonly IContractDL _contractDL;
        private readonly IBookingDL _bookingDL;

        public PaymentBL(IPaymentDL paymentDL, IContractDL contractDL, IBookingDL bookingDL) : base(paymentDL)
        {
            _paymentDL = paymentDL;
            _contractDL = contractDL;
            _bookingDL = bookingDL;
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
                payment.Description = $"Bạn có lịch đóng tiền thuê căn hộ {apartment.RoomNumber} từ ngày hôm nay đến ngày mùng 10";
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
                int days = (booking.EndDate - booking.StartDate).Days + 1;

                if (booking.BookingType == BookingType.Monthly)
                {
                    int daysInMonth = DateTime.DaysInMonth(booking.EndDate.Year, booking.EndDate.Month);
                    if (days < daysInMonth)
                    {
                        payment.Amount = (days / daysInMonth) * service.MonthlyPrice;
                    }
                    else if (days == daysInMonth)
                    {
                        payment.Amount = service.MonthlyPrice;
                    }
                    else
                    {
                        int leftDays = days - daysInMonth;
                        payment.Amount = service.MonthlyPrice + (leftDays / daysInMonth) * service.MonthlyPrice;
                    }
                }
                else
                {
                    payment.Amount = service.Price * days;
                }
            }

            await _paymentDL.AddPaymentsAsync(payments);

            return payments;
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
