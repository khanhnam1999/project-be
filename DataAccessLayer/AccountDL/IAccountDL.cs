using CommonDataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer
{
    public interface IAccountDL : IBaseDL<Account>
    {
        public Account Authenticate(string username);

        public void SaveToken(Guid id, string token);
    }
}
