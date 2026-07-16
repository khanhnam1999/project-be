using CommonDataLayer.Entities;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicLayer
{
    public class IncidentBL : BaseBL<Incident>, IIncidentBL
    {
        private readonly IIncidentDL _incidentDL;
        public IncidentBL(IIncidentDL incidentDL) : base(incidentDL)
        {
            _incidentDL = incidentDL;
        }
    }
}
