using NotificationPortal.ApiModels;
using NotificationPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace NotificationPortal.ApiRepositories
{
    public class ApplicationApiRepo
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        // CheckStatus of the apps then RefreshStatusInDatabase for each apps
        // based on the input referenceIDs of apps
        // and return the result for Api
        public List<ApplicationStatus> RefreshApplicationStatuses(string[] referenceIDs)
        {
            // get the referenceIds and url of the apps
            var apps = _context.Application
                .Where(a => referenceIDs.Contains(a.ReferenceID))
                .Select(a => new { ReferenceID = a.ReferenceID, URL = a.URL }).ToList();

            // loop through all apps and check the status
            List<ApplicationStatus> listOfAppStatuses = new List<ApplicationStatus>();
            foreach (var app in apps)
            {
                listOfAppStatuses.Add(
                    new ApplicationStatus()
                    {
                        ReferenceID = app.ReferenceID,
                        Status = CheckStatus(app.URL) ? Key.STATUS_APPLICATION_ONLINE : Key.STATUS_APPLICATION_OFFLINE
                    });
            }

            // refresh the status of the apps in the database
            RefreshStatusInDatabase(listOfAppStatuses);

            return listOfAppStatuses;
        }

        // refresh the status of the apps in the database
        public void RefreshStatusInDatabase(List<ApplicationStatus> listOfAppStatuses)
        {
            try
            {
                int onlineStatusID = _context.Status
                    .Where(s => s.StatusType.StatusTypeName == Key.STATUS_TYPE_APPLICATION
                    && s.StatusName == Key.STATUS_APPLICATION_ONLINE).FirstOrDefault().StatusID;
                int offlineStatusID = _context.Status
                    .Where(s => s.StatusType.StatusTypeName == Key.STATUS_TYPE_APPLICATION
                    && s.StatusName == Key.STATUS_APPLICATION_OFFLINE).FirstOrDefault().StatusID;

                // loop through all apps and replace the value of the Status with new value from CheckStatus method
                foreach (var app in listOfAppStatuses)
                {
                    var updatingApp = _context.Application
                    .Where(a => app.ReferenceID == a.ReferenceID).FirstOrDefault();

                    if (app.Status == Key.STATUS_APPLICATION_ONLINE)
                    {
                        updatingApp.StatusID = onlineStatusID;
                    }
                    else
                    {
                        updatingApp.StatusID = offlineStatusID;
                    }
                }
                _context.SaveChanges();
            }
            catch (Exception)
            {
                // TODO: decide how to handle exception when there is a problem in the database
            }
        }

        // check if application is online
        public bool CheckStatus(string url)
        {
            // request for a response to a url and check to recieve an OK status
            try
            {
                WebRequest request = WebRequest.Create(url);
                var x = request.GetResponse();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return true;
                }
            }
            catch (Exception)
            {
                // TODO: decide how to handle exception when there is no connection
            }
            return false;
        }

        // get all apps associated with input server reference ids
        public List<ApplicationListItem> GetApplications(string[] serverReferenceIDs)
        {
            List<ApplicationListItem> apps = _context.Application
                .Where(a => a.Servers.Where(s => serverReferenceIDs.Contains(s.ReferenceID)).Count() > 0)
                .Select(a=> new ApplicationListItem() {
                    ApplicationName = a.ApplicationName,
                    ReferenceID = a.ReferenceID
                }).ToList();
            return apps;
        }
    }
}