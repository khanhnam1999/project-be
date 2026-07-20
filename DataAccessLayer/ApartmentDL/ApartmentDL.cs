using CommonDataLayer.DTO;
using CommonDataLayer.Entities;
using CommonDataLayer.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer
{
    public class ApartmentDL : BaseDL<Apartment>, IApartmentDL
    {
        private readonly DbSet<Apartment> _dbSet;
        private readonly DbSet<Contract> _dbSetContract;
        public ApartmentDL(CondoContext condoContext) : base(condoContext)
        {
            _dbSet = condoContext.Set<Apartment>();
            _dbSetContract = condoContext.Set<Contract>();
        }

        public override Apartment GetById(Guid id)
        {
            var query = _dbSet.Where(NotDeleted<Apartment>())
               .Include(x => x.Contracts.Where(a => !a.IsDeleted))
                    .ThenInclude(c => c.ContractResidents.Where(cr => !cr.IsDeleted))
               .Include(x => x.Incidents.Where(a => !a.IsDeleted));

            return query.FirstOrDefault(x => x.ApartmentId == id);
        }

        public async Task<ApartmentReportDto> GetApartmentReport()
        {
            ApartmentReportDto results = new ApartmentReportDto();

            results.EmptyCount = await _dbSet.CountAsync(a => !a.IsDeleted && a.Status == ApartmentStatusEnum.Available);
            results.MaintenanceCount = await _dbSet.CountAsync(a => !a.IsDeleted && a.Status == ApartmentStatusEnum.Maintenance);
            results.OwnedCount = await _dbSetContract.CountAsync(a => !a.IsDeleted && a.Type == ContractTypeEnum.Cash);
            results.RentedCount = await _dbSetContract.CountAsync(a => !a.IsDeleted && a.Type == ContractTypeEnum.Rental);
            results.TotalCount = await _dbSet.CountAsync(a => !a.IsDeleted);

            return results;
        }
    }
}
