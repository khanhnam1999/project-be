using CommonDataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer
{
    public interface IPaymentDL : IBaseDL<Payment>
    {
        Task AddPaymentsAsync(List<Payment> payments);
    }
}
