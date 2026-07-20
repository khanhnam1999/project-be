using CommonDataLayer.DTO;
using CommonDataLayer.Entities;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicLayer
{
    public class ApartmentBL : BaseBL<Apartment>, IApartmentBL
    {
        private readonly IApartmentDL _apartmentDL;

        public ApartmentBL(IApartmentDL apartmentDL) : base(apartmentDL) => _apartmentDL = apartmentDL;

        public async Task<ApartmentReportDto> GetApartmentReport()
        {
            return await _apartmentDL.GetApartmentReport();
        }
    }
}
