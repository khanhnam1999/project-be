using CommonDataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class ContractDL : BaseDL<Contract>, IContractDL
    {
        private readonly DbSet<Contract> _dbSet;
        private readonly DbSet<ContractResident> _dbSetCR;
        private readonly CondoContext _condoContext;
        public ContractDL(CondoContext condoContext) : base(condoContext)
        {
            _condoContext = condoContext;
            _dbSet = condoContext.Set<Contract>();
            _dbSetCR = condoContext.Set<ContractResident>();
        }

        public Guid UpdateResidentToContract(ContractResident contractResident)
        {
            ContractResident cr = _dbSetCR
                .Where(x => x.ContractId == contractResident.ContractId
                        && x.ResidentId == contractResident.ResidentId
                        && !x.IsDeleted)
                .FirstOrDefault();
            cr.ResidentType = contractResident.ResidentType;
            cr.IsDeleted = contractResident.IsDeleted;
            cr.ModifiedDate = contractResident.ModifiedDate;

            _dbSetCR.Update(cr);
            _condoContext.SaveChanges();

            return cr.ContractId;
        }

        public override Contract GetById(Guid id)
        {
            var query = _dbSet.Where(NotDeleted<Contract>())
                .Include(x => x.Apartment)
                .Include(x => x.ContractResidents.Where(a => !a.IsDeleted));
            return query.FirstOrDefault(x => x.ContractId == id);
        }
    }
}
