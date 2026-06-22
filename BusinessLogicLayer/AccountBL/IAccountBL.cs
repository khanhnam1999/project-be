using CommonDataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicLayer
{
    public interface IAccountBL : IBaseBL<Account>
    {
        public Account Authenticate(Login login);

        public Guid Register(Account account);
    }
}
