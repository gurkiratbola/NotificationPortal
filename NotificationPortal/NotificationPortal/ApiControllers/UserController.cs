using NotificationPortal.ApiModels;
using NotificationPortal.ApiRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NotificationPortal.ApiControllers
{
    public class UserController : ApiController
    {
        UserApiRepo _uApiRepo = new UserApiRepo();

        // GET: api/User
        public List<ApplicationListItem> GET(string id)
        {
            List<ApplicationListItem> apps = new List<ApplicationListItem>();
            if (id != null)
            {
                apps = _uApiRepo.GetApplicationsByClient(id);
            }
            return apps;
        }
    }
}
