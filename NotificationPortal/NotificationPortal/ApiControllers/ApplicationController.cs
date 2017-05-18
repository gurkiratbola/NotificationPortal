using NotificationPortal.ApiModels;
using NotificationPortal.ApiRepositories;
using NotificationPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NotificationPortal.ApiControllers
{
    public class ApplicationController : ApiController
    {
        ApplicationApiRepo _aApiRepo = new ApplicationApiRepo();

        public List<ApplicationListItem> POST(string[] serverReferenceIDs)
        {
            List<ApplicationListItem> apps=new List<ApplicationListItem>();
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
