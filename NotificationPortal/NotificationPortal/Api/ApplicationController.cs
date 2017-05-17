using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NotificationPortal.Api
{
    public class ApplicationController : ApiController
    {
        ApplicationApiRepo _aApiRepo = new ApplicationApiRepo();

        public List<Application> POST(string[] serverReferenceIDs)
        {
            List<Application> apps=new List<Application>();
            if (serverReferenceIDs != null)
            {
                apps = _aApiRepo.GetApplications(serverReferenceIDs);
            }
            return apps;
        }

        public List<ApplicationStatus> PUT(string[] applicationReferenceIDs)
        {
            List<ApplicationStatus> appStatuses = _aApiRepo.RefreshApplicationStatuses(applicationReferenceIDs);
            return appStatuses;
        }
    }
}
