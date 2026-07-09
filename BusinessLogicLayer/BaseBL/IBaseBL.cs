using CommonDataLayer.DTO;
using CommonDataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicLayer
{
    public interface IBaseBL<T> where T : BaseEntity
    {
        // Lấy tất cả
        IEnumerable<T> GetAll();

        // Phân trang, tìm kiếm, sắp xếp
        FilterResult<T> FilterData(FilterData filterData);

        // Lấy theo ID
        T GetById(Guid id);

        // Thêm mới
        Guid Add(T entity);

        // Cập nhật
        Guid Update(Guid id, T entity);

        // Xóa
        int Delete(List<Guid> ids);

        // Xóa cứng
        int DeleteHard(List<Guid> ids);

        // Khôi phục
        int Restore(List<Guid> ids);
    }
}
