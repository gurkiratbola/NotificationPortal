using NotificationPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace NotificationPortal.Api
{
    public class ApplicationApiRepo
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        public List<ApplicationStatus> RefreshApplicationStatuses(string[] referenceIDs)
        {
            var apps = _context.Application
                .Where(a => referenceIDs.Contains(a.ReferenceID))
                .Select(a => new { ReferenceID=a.ReferenceID, URL=a.URL }).ToList();

            List<ApplicationStatus> listOfAppStatuses = new List<ApplicationStatus>();
            foreach (var app in apps)
            {
                listOfAppStatuses.Add(
                    new ApplicationStatus() {
                        ReferenceID = app.ReferenceID,
                        Status = CheckStatus(app.URL) ? Key.STATUS_APPLICATION_ONLINE : Key.STATUS_APPLICATION_OFFLINE
                    });
            }

            RefreshStatusInDatabase(listOfAppStatuses);

            return listOfAppStatuses;
        }

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
            WebRequest request = WebRequest.Create(url);

            // request for a response to a surl 200 times until we recieve an OK status
            for (int i = 0; i < 200; i++)
            {
                try
                {
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
            }
            return false;
        }
    }
}