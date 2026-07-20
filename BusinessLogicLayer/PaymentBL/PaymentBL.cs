

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

        public PaymentBL(IPaymentDL paymentDL, IContractDL contractDL) : base(paymentDL)
        {
            _paymentDL = paymentDL;
            _contractDL = contractDL;
        }

        public async Task<List<Payment>> GeneratePayments()
        {
            List<Payment> payments = new List<Payment>();
            List<Contract> contracts = await _contractDL.GetListContractsWithoutCashType();

            foreach (Contract contract in contracts)
            {
                Payment payment = new Payment();
                payment.Title = "Đóng tiền nhà";

                ContractResident contractResident = contract.ContractResidents.FirstOrDefault(x => x.ResidentType == ResidentTypeEnum.HomeOwner);

                payment.ResidentId = contractResident.ResidentId;
                payment.Amount = contract.Apartment.RentPrice;
                DateTime now = DateTime.Now;
                payment.PaymentDeadline = now.AddDays(10);
                payment.Description = "Bạn có lịch đóng tiền thuê nhà";
            }

            await _paymentDL.AddPaymentsAsync(payments);

            return payments;
        }

        public async Task<List<PaymentReportDto>> GetReport(DateTime startDate, DateTime endDate, string periodType)
        {
            if (startDate > endDate) throw new Exception("Ngày bắt đầu không được lớn hơn ngày kết thúc");

            if(periodType == "monthly")
            {
                return await _paymentDL.GetReportByMonthly(startDate, endDate);
            }
            else if(periodType == "weekly")
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
