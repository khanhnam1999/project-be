using CommonDataLayer.DTO;
using CommonDataLayer.Entities;
using System.Linq.Expressions;

namespace DataAccessLayer
{
    public interface IBaseDL<T> where T : BaseEntity
    {
        // Cập nhật hoàn tất
        void Complete(T entity);

        // Lấy tất cả
        IEnumerable<T> GetAll();

        // Phân trang và lọc dữ liệu
        FilterResult<T> FilterData(FilterData filterData);

        // Function trả về query tổng thể cho filter
        IQueryable<T> GetQueryFilterData(FilterData filterData);

        // Lấy theo ID
        T GetById(Guid id);

        // Thêm mới
        Guid Add(T entity);

        // Cập nhật
        Guid Update(Guid id, T entity);

        // Xóa cứng
        int DeleteHard(List<Guid> ids);

        // Khôi phục / Xóa mềm
        int ChangeDeleteStatus(List<Guid> ids, bool isDeleted);
    }
}
