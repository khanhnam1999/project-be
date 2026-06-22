using CommonDataLayer.Entities;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicLayer
{
    public class ServiceBL : BaseBL<Service>, IServiceBL
    {
        private readonly IServiceDL _serviceDL;
        public ServiceBL(IServiceDL serviceDL) : base(serviceDL) => _serviceDL = serviceDL;
    }
}
