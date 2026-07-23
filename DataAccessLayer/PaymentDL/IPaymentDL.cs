using CommonDataLayer.DTO;
using CommonDataLayer.Entities;
using CommonDataLayer.Enum;

namespace DataAccessLayer
{
    public interface IPaymentDL : IBaseDL<Payment>
    {
        Task AddPaymentsAsync(List<Payment> payments);
        FilterResult<PaymentListDto> FilterPayments(FilterData filterData);

        Task<List<PaymentReportDto>> GetReportByMonthly(DateTime startDate, DateTime endDate);
        Task<List<PaymentReportDto>> GetReportByWeekly(DateTime startDate, DateTime endDate);
        Task<List<PaymentInvoiceDto>> GetInvoicesByAccountIdAsync(Guid accountId);
        Task<List<Payment>> CheckoutAsync(Guid accountId, IReadOnlyCollection<Guid> paymentIds,
            PaymentMethodEnum paymentMethod, Guid transactionId, string referenceCode);
        Task<int> ConfirmTransactionAsync(Guid transactionId);
    }
}
