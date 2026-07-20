using CommonDataLayer.DTO;
using CommonDataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicLayer
{
    public interface IApartmentBL : IBaseBL<Apartment>
    {
        Task<ApartmentReportDto> GetApartmentReport();
    }
}
