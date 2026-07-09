using CommonDataLayer.DTO;
using CommonDataLayer.Entities;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public class BaseBL<T> : IBaseBL<T> where T : BaseEntity
    {
        private IBaseDL<T> _baseDL;

        public BaseBL(IBaseDL<T> baseDL)
        {
            _baseDL = baseDL;
        }

        public Guid Add(T entity)
        {
            return _baseDL.Add(entity);
        }

        public int Delete(List<Guid> ids)
        {
            return _baseDL.ChangeDeleteStatus(ids, true);
        }

        public int Restore(List<Guid> ids)
        {
            return _baseDL.ChangeDeleteStatus(ids, false);
        }

        public int DeleteHard(List<Guid> id)
        {
            return _baseDL.DeleteHard(id);
        }

        public IEnumerable<T> GetAll()
        {
            return _baseDL.GetAll();
        }

        public FilterResult<T> FilterData(FilterData filterData)
        {
            return _baseDL.FilterData(filterData);
        }

        public T GetById(Guid id)
        {
            return _baseDL.GetById(id);
        }

        public Guid Update(Guid id, T entity)
        {
            return _baseDL.Update(id, entity);
        }
    }
}
