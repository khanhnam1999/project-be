using CommonDataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer
{
    public interface IContractDL : IBaseDL<Contract>
    {
        Guid UpdateResidentToContract(ContractResident contractResident);
    }
}
