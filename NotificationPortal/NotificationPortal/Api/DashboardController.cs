using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NotificationPortal.Api
{
    public class DashboardController : ApiController
    {
        private readonly DashboardApiRepo _dApiRepo = new DashboardApiRepo();
        // POST: api/Dashboard
        public DashboardIndexFiltered Post([FromBody] IndexBody model)
        {
            DashboardIndexFiltered result = _dApiRepo.GetFilteredAndSortedDasboard(model);
            return result;
        }
    }
}