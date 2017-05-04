using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NotificationPortal.Models;
using NotificationPortal.ViewModels;

namespace NotificationPortal.Repositories
{
    public class DashboardRepo
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        public IEnumerable<DashboardVM> GetDashBoard()
        {
            IEnumerable<DashboardVM> dashboard = from notif in _context.Notification
                                                 select new DashboardVM()
                                                 {
                                                     //ApplicationID = app.ApplicationID,
                                                     //ApplicationName = notif.Applications.ToString(),
                                                     NotificationID = notif.NotificationID,
                                                     NotificationTypeName = notif.NotificationType.NotificationTypeName,
                                                     NotificationHeading = notif.NotificationHeading,
                                                     ThreadID = notif.ThreadID,
                                                     ReferenceID = notif.ReferenceID,
                                                     StatusID = notif.Status.StatusID,
                                                     StatusName = notif.Status.StatusName,
                                                     //ServerID = notif.Server.ServerID,
                                                     //ServerName = notif.Servers.Tostring(),
                                                     LevelOfImpactID = notif.LevelOfImpact.LevelOfImpactID,
                                                     LevelOfImpactName = notif.LevelOfImpact.Level,
                                                     //URL = app.URL,
                                                     StartDateTime = notif.StartDateTime
                                                 };
            return dashboard;
        }
    }
}