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

        public Guid SetPwd(string identityNumber, string email, string password)
        {
            Account account = _dbSet.Where(NotDeleted<Account>())
                .FirstOrDefault(x => x.IdentityNumber == identityNumber && x.Email == email);

            if (account == null) throw new Exception("Lỗi đổi mật khẩu");

            account.Password = password;
            Complete(account);

            return account.AccountId;
        }

        public Account Authenticate(string identityNunber)
        {
            Account account = _dbSet
                .Where(NotDeleted<Account>())
                .FirstOrDefault(a => a.IdentityNumber == identityNunber);
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
