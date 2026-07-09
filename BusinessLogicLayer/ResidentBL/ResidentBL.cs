using CommonDataLayer.Entities;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicLayer
{
    public class ResidentBL : BaseBL<Resident>, IResidentBL
    {
        private readonly IResidentDL _residentDL;
        public ResidentBL(IResidentDL residentDL) : base(residentDL)
        {
            _residentDL = residentDL;
        }
    }
}
