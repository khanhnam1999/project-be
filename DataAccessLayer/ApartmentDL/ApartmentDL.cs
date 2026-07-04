using CommonDataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer
{
    public class ApartmentDL : BaseDL<Apartment>, IApartmentDL
    {
        private readonly DbSet<Apartment> _dbSet;
        public ApartmentDL(CondoContext condoContext) : base(condoContext)
        {
            _dbSet = condoContext.Set<Apartment>();
        }

        public override Apartment GetById(Guid id)
        {
            var query = _dbSet.Where(NotDeleted<Apartment>())
               .Include(x => x.Contracts.Where(a => !a.IsDeleted))
               .Include(x => x.Incidents.Where(a => !a.IsDeleted));

            return query.FirstOrDefault(x => x.ApartmentId == id);
        }
    }
}
