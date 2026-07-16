using CommonDataLayer.DTO;
using CommonDataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class IncidentDL : BaseDL<Incident>, IIncidentDL
    {
        private readonly CondoContext _condoContext;
        private readonly DbSet<Incident> _dbSet;
        public IncidentDL(CondoContext condoContext) : base(condoContext)
        {
            _condoContext = condoContext;
            _dbSet = condoContext.Set<Incident>();
        }

        public override FilterResult<Incident> FilterData(FilterData filterData)
        {
            var query = _dbSet.AsQueryable();

            query = query.Include(x => x.Apartment)
                .Include(a => a.Resident)
                    .ThenInclude(ax => ax.Account);

            if (filterData.Conditions.Any())
            {
                foreach (var condition in filterData.Conditions)
                {
                    if (condition.Key == "Status")
                    {
                        query = query.Where(r => r.Status == condition.IncidentStatusValue);
                    }
                    else if (condition.GuidValue != null)
                    {
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

            FilterResult<Incident> filterResult = new FilterResult<Incident>();

            filterResult.TotalRecords = query.Count();

            if (filterData.Page != 0)
            {
                query = query.Skip((filterData.Page - 1) * filterData.Limit).Take(filterData.Limit);
            }

            filterResult.Results = query.ToList();

            return filterResult;
        }
    }
}
