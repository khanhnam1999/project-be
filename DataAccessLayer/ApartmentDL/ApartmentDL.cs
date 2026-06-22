using CommonDataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer
{
    public class ApartmentDL : BaseDL<Apartment>, IApartmentDL
    {
        public ApartmentDL(CondoContext condoContext) : base(condoContext)
        {
        }
    }
}
