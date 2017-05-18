using NotificationPortal.ApiModels;
using NotificationPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotificationPortal.ApiRepositories
{
    public class UserApiRepo
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        // get all apps associated with input client reference id
        public List<ApplicationListItem> GetApplicationsByClient(string clientReferenceID)
        {
            List<ApplicationListItem> apps = _context.Application
                .Where(a => a.Client.ReferenceID == clientReferenceID)
                .Select(a => new ApplicationListItem()
                {
                    ApplicationName = a.ApplicationName,
                    ReferenceID = a.ReferenceID
                }).ToList();
            return apps;
        }
    }
}