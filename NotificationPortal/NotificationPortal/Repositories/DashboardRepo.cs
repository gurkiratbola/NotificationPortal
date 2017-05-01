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
            IEnumerable<DashboardVM> dashboard = from app in _context.Application
                                                 from notif in _context.Notification
                                                 where app.ApplicationID == notif.ApplicationID
                                                 select new DashboardVM()
                                                 {
                                                     ApplicationID = app.ApplicationID,
                                                     ApplicationName = app.ApplicationName,
                                                     NotificationID = notif.NotificationID,
                                                     NotificationTypeName = notif.NotificationType.NotificationTypeName,
                                                     NotificationHeading = notif.NotificationHeading,
                                                     ThreadID = notif.ThreadID,
                                                     ReferenceID = notif.ReferenceID,
                                                     StatusID = app.Status.StatusID,
                                                     StatusName = app.Status.StatusName,
                                                     ClientID = app.Client.ClientID,
                                                     ClientName = app.Client.ClientName,
                                                     ServerID = notif.Server.ServerID,
                                                     ServerName = notif.Server.ServerName,
                                                     LevelOfImpactID = notif.LevelOfImpact.LevelOfImpactID,
                                                     LevelOfImpactName = notif.LevelOfImpact.Level,
                                                     URL = app.URL,
                                                     StartDateTime = notif.StartDateTime,
                                                     EndDateTime = notif.EndDateTime
                                                 };
            return dashboard;
        }
    }
}