using CommonDataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicLayer
{
    public interface IContractBL : IBaseBL<Contract>
    {
        Guid UpdateResidentToContract(ContractResident contractResident);
    }
}
