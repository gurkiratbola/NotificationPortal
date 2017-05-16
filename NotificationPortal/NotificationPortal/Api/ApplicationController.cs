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
        public List<ApplicationStatus> POST(string[] applicationReferenceIDs)
        {
            List<ApplicationStatus> appStatuses = _aApiRepo.RefreshApplicationStatuses(applicationReferenceIDs);
            return appStatuses;
        }
    }
}
