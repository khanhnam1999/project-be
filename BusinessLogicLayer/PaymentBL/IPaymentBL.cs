using CommonDataLayer.Entities;

namespace BusinessLogicLayer
{
    public interface IPaymentBL : IBaseBL<Payment>
    {
        Task<List<Payment>> GeneratePayments();
    }
}
