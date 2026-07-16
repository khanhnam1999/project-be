using CommonDataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer
{
    public class PaymentDL : BaseDL<Payment>, IPaymentDL
    {
        private readonly CondoContext _condoContext;
        private readonly DbSet<Payment> _dbSet;

        public PaymentDL(CondoContext condoContext) : base (condoContext)
        {
            _condoContext = condoContext;
            _dbSet = condoContext.Set<Payment>();
        }

        public async Task AddPaymentsAsync(List<Payment> payments)
        {
            using var transaction = await _condoContext.Database.BeginTransactionAsync();
            try
            {
                // Thêm danh sách payment
                await _dbSet.AddRangeAsync(payments);
                await _condoContext.SaveChangesAsync();

                // Commit transaction nếu thành công
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                // Rollback nếu có lỗi
                await transaction.RollbackAsync();
                throw; // hoặc log lỗi
            }
        }
    }
}
