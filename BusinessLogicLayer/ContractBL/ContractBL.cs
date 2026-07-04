using CommonDataLayer.Entities;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public class ContractBL : BaseBL<Contract>, IContractBL
    {
        private IContractDL _contracDL;
        public ContractBL(IContractDL contracDL) : base(contracDL)
        {
            _contracDL = contracDL;
        }

        public Guid UpdateResidentToContract(ContractResident contractResident)
        {
            return _contracDL.UpdateResidentToContract(contractResident);
        }
    }
}
