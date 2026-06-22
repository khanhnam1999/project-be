using CommonDataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer
{
    public class ServiceDL : BaseDL<Service>, IServiceDL
    {
        public ServiceDL(CondoContext condoContext) : base(condoContext) { }
    }
}
