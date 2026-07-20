using CommonDataLayer.DTO;
using CommonDataLayer.Entities;

namespace DataAccessLayer
{
    public interface IApartmentDL : IBaseDL<Apartment>
    {
        Task<ApartmentReportDto> GetApartmentReport();
    }
}
