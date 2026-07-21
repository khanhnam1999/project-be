using CommonDataLayer.DTO;
using CommonDataLayer.Entities;

namespace BusinessLogicLayer
{
    public interface IPaymentBL : IBaseBL<Payment>
    {
        Task<List<Payment>> GeneratePayments();

        Task<List<Payment>> GenerateBookingPayments();

        Task<List<PaymentReportDto>> GetReport(DateTime startDate, DateTime endDate, string periodType);
        Task<List<PaymentInvoiceDto>> GetMyInvoices(Guid accountId);
        Task<CheckoutResponseDto> Checkout(Guid accountId, CheckoutRequestDto request);
        Task<int> ConfirmTransaction(Guid transactionId);
    }
}
