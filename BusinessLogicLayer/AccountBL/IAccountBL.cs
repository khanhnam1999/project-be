using CommonDataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicLayer
{
    public interface IAccountBL : IBaseBL<Account>
    {
        Account Authenticate(Login login);

        Guid Register(Account account);

        Guid SetPwd(string identityNumber, string email, string password);

        string GetEmail(string identityNumber);
    }
}
