using CommonDataLayer.DTO;
using CommonDataLayer.Entities;

namespace DataAccessLayer
{
    public interface IPaymentDL : IBaseDL<Payment>
    {
        Task AddPaymentsAsync(List<Payment> payments);

        Task<List<PaymentReportDto>> GetReportByMonthly(DateTime startDate, DateTime endDate);
        Task<List<PaymentReportDto>> GetReportByWeekly(DateTime startDate, DateTime endDate);
    }
}
