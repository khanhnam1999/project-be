using CommonDataLayer.DTO;
using CommonDataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class ResidentDL : BaseDL<Resident>, IResidentDL
    {
        private readonly DbSet<Resident> _dbSet;
        private readonly IBaseDL<Resident> _baseDL;
        public ResidentDL(CondoContext condoContext, IBaseDL<Resident> baseDL) : base(condoContext)
        {
            _dbSet = condoContext.Set<Resident>();
            _baseDL = baseDL;
        }

        public override Resident GetById(Guid id)
        {
            var query = _dbSet.Where(NotDeleted<Resident>())
                //.Include(x => x.Account.Where(a => !a.IsDeleted))
                .Include(x => x.ContractResidents.Where(a => !a.IsDeleted));
            return query.FirstOrDefault(x => x.ResidentId == id);
        }

        public override FilterResult<Resident> FilterData(FilterData filterData)
        {
            var query = _dbSet.AsQueryable();
            query = query.Include(x => x.Account)
                .Where(a => !a.Account.IsDeleted);
            if (filterData.Conditions.Any())
            {
                foreach (var condition in filterData.Conditions)
                {
                    if (condition.Key == "FullName")
                    {
                        query = query.Where(r => r.Account.FullName.Contains(condition.Value));
                    }
                    else if(condition.Key == "IdentityNumber")
                    {
                        query = query.Where(r => r.Account.IdentityNumber.Contains(condition.Value));
                    }
                    else if(condition.GuidValue != null) {
                        var lambda = CreateLambda(condition.Key, condition.GuidValue);
                        query = query.Where(lambda);
                    }
                    else
                    {
                        var lambda = CreateLambda(condition.Key, condition.Value, "Contains");
                        query = query.Where(lambda);
                    }
                }

            }
            query = ApplyOrdering(query, filterData.SortName, filterData.SortMethod)
                .Where(NotDeleted<Resident>());

            FilterResult<Resident> filterResult = new FilterResult<Resident>();

            filterResult.TotalRecords = query.Count();

            // Nếu Page = 0 thì lấy full data k cần phân trang
            if (filterData.Page != 0)
            {
                query = query.Skip((filterData.Page - 1) * filterData.Limit).Take(filterData.Limit);
            }

            filterResult.Results = query.ToList();

            return filterResult;
        }
    }
}
