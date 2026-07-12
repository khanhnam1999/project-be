using CommonDataLayer.DTO;
using CommonDataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

namespace DataAccessLayer
{
    public class BaseDL<T> : IBaseDL<T> where T : BaseEntity
    {
        private readonly CondoContext _context;
        private readonly DbSet<T> _dbSet;

        public BaseDL(CondoContext condoContext)
        {
            _context = condoContext;
            _dbSet = condoContext.Set<T>();
        }

        public Guid Add(T entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
            return GetPrimaryKeyValue(entity);
        }

        public void Complete(T entity)
        {
            _dbSet.Update(entity);
            _context.SaveChanges();
        }

        public int ChangeDeleteStatus(List<Guid> ids, bool isDeleted)
        {
            int count = 0;
            foreach (Guid id in ids)
            {
                T entity = _dbSet.Find(id);
                if (entity != null && typeof(T).GetProperty("IsDeleted") != null)
                {
                    typeof(T).GetProperty("IsDeleted").SetValue(entity, isDeleted);
                    typeof(T).GetProperty("ModifiedDate").SetValue(entity, DateTime.Now);
                    count++;
                }
            }
            if (count != ids.Count()) throw new Exception($"Không xóa được hết {ids.Count()} dữ liệu. Xin vui lòng kiểm tra lại");
            return _context.SaveChanges();
        }

        public int DeleteHard(List<Guid> ids)
        {
            int count = 0;
            foreach (Guid id in ids)
            {
                T entity = _dbSet.Find(id);
                if (entity != null && typeof(T).GetProperty("IsDeleted") != null)
                {
                    _dbSet.Remove(entity);
                    count++;
                }
            }

            if (count != ids.Count()) throw new Exception($"Không xóa được hết {ids.Count()} dữ liệu. Xin vui lòng kiểm tra lại");
            return _context.SaveChanges();
        }

        public IEnumerable<T> GetAll()
        {
            T[] list = _dbSet.Where(NotDeleted<T>()).ToArray();
            return list;
        }

        public IQueryable<T> GetQueryFilterData(FilterData filterData)
        {
            var query = _dbSet.AsQueryable();

            query = ApplyOrdering(query, filterData.SortName, filterData.SortMethod);

            if (filterData.Conditions.Any())
            {
                foreach (var condition in filterData.Conditions)
                {
                    if (condition.GuidValue != null)
                    {
                        var lambda = CreateLambda(condition.Key, condition.GuidValue);
                        query = query.Where(lambda);
                    }
                    else if (condition.Value != null)
                    {
                        var lambda = CreateLambda(condition.Key, condition.Value, "Contains");
                        query = query.Where(lambda);
                    }
                    else
                    {
                        throw new Exception("Không tồn tại value");
                    }
                }
            }

            // Lấy những dữ liệu có isDelete != 1
            query = query.Where(NotDeleted<T>());

            return query;
        }

        public virtual FilterResult<T> FilterData(FilterData filterData)
        {
            var query = GetQueryFilterData(filterData);

            FilterResult<T> filterResult = new FilterResult<T>();

            filterResult.TotalRecords = query.Count();

            // Nếu Page = 0 thì lấy full data k cần phân trang
            if (filterData.Page != 0)
            {
                query = query.Skip((filterData.Page - 1) * filterData.Limit).Take(filterData.Limit);
            }

            filterResult.Results = query.ToList();
            return filterResult;
        }

        public virtual T GetById(Guid id)
        {
            var query = _dbSet.Where(NotDeleted<T>());

            var lambda = CreateLambda(GetPrimaryKey<T>().Name, id);
            var sql = query.ToQueryString();
            Console.WriteLine(sql);
            T result = query.FirstOrDefault(lambda);
            return result;
        }

        public Guid Update(Guid id, T entity)
        {
            PropertyInfo[] props = typeof(T).GetProperties();
            Guid entityId = GetPrimaryKeyValue(entity);
            if (id != entityId) throw new Exception("Không tìm thấy dữ liệu cần chỉnh sửa");

            var existing = GetById(id);
            if (existing == null) throw new Exception("Không tìm thấy dữ liệu cần chỉnh sửa");

            foreach (PropertyInfo prop in props)
            {
                if (prop.Name == "CreatedDate" || prop.Name == "IsDeleted") continue;
                var value = prop.GetValue(entity);
                if (value != null) prop.SetValue(existing, value, null);
            }

            Complete(existing);

            return id;
        }

        protected static Expression<Func<T, bool>> NotDeleted<T>() where T : BaseEntity => t => !t.IsDeleted;

        protected IQueryable<T> ApplyOrdering(IQueryable<T> source, string propertyName, string sortMethod)
        {
            var propertyInfo = typeof(T).GetProperty(propertyName);

            if (propertyInfo == null) return source;

            var parameter = Expression.Parameter(typeof(T), "u");

            var propertyAccess = Expression.MakeMemberAccess(parameter, propertyInfo);

            var orderByExp = Expression.Lambda(propertyAccess, parameter);

            string methodName = sortMethod != "DESC" ? "OrderBy" : "OrderByDescending";

            var resultExp = Expression.Call(
                typeof(Queryable),
                methodName,
                new Type[] {
                    typeof(T),
                    propertyInfo.PropertyType
                },
                source.Expression,
                Expression.Quote(orderByExp)
            );

            return source.Provider.CreateQuery<T>(resultExp);
        }

        public static Guid GetPrimaryKeyValue(T entity)
        {
            PropertyInfo[] props = typeof(T).GetProperties();
            var primaryKeyProp = GetPrimaryKey<T>();

            return (Guid)primaryKeyProp.GetValue(entity);
        }

        public static PropertyInfo GetPrimaryKey<T>()
        {
            PropertyInfo[] props = typeof(T).GetProperties();
            var primaryKeyProp = props.FirstOrDefault(prop => prop.GetCustomAttributes(typeof(KeyAttribute), true).Count() > 0);

            if (primaryKeyProp == null) throw new Exception($"Entity {typeof(T).Name} chưa được định dạng primary key");

            return primaryKeyProp;
        }

        protected static Expression<Func<T, bool>> CreateLambda(string key, object value, string compareType = "Equal")
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, key);

            // khi value = null nghĩa là (x => x.AccountId)
            //if (value == null)
            //{
            //    return Expression.Lambda<Func<T, bool>>(
            //        Expression.Convert(property, typeof(object)),
            //        parameter
            //    );
            //}

            // Khi value != null nghĩa là (x => x.AccountId == value)
            var constant = Expression.Constant(value);
            if (compareType != "Equal")
            {
                var containsMethod = typeof(string).GetMethod(compareType, new[] { typeof(string) });
                var body = Expression.Call(property, containsMethod, constant);
                return Expression.Lambda<Func<T, bool>>(body, parameter);
            }
            else
            {
                var body = Expression.Equal(property, constant);
                return Expression.Lambda<Func<T, bool>>(body, parameter);

            }
        }
    }
}
