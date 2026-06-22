using CommonDataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer
{
    public class AccountDL : BaseDL<Account>, IAccountDL
    {
        private readonly CondoContext _condoContext;
        private readonly DbSet<Account> _dbSet;
        public AccountDL(CondoContext condoContext) : base(condoContext)
        {
            _condoContext = condoContext;
            _dbSet = condoContext.Set<Account>();
        }

        public Account Authenticate(string username)
        {
            Account account = _dbSet.FirstOrDefault(a => a.Username == username);
            if (account == null) throw new Exception("Không đăng nhập được");

            return account;
        }
    }
}
