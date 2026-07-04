using CommonDataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer
{
    public interface IAccountDL : IBaseDL<Account>
    {
        public Account Authenticate(string identityNunber);

        public void SaveToken(Guid id, string token);

        Guid SetPwd(string identityNumber, string email, string password);
    }
}
