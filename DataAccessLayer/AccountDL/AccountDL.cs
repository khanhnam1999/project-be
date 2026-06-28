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

        public void SaveToken(Guid id, string token)
        {
            Account account = _dbSet.FirstOrDefault(a => a.AccountId == id);
            if (account == null) throw new Exception("Không lưu được token");
            account.Token = token;
            Complete(account);
        }
    }
}
