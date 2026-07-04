using CommonDataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.ResidentDL
{
    public class ResidentDL : BaseDL<Resident>, IResidentDL
    {
        private readonly DbSet<Resident> _dbSet;
        public ResidentDL(CondoContext condoContext) : base(condoContext)
        {
            _dbSet = condoContext.Set<Resident>();
        }

        public override Resident GetById(Guid id)
        {
            var query = _dbSet.Where(NotDeleted<Resident>())
                .Include(x => x.ContractResidents.Where(a => !a.IsDeleted));
            return query.FirstOrDefault(x => x.ResidentId == id);
        }
    }
}
