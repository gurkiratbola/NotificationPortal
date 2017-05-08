using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NotificationPortal.Models;
using NotificationPortal.ViewModels;
using System.Security.Principal;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace NotificationPortal.Repositories
{
    public class DashboardRepo
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        public IEnumerable<DashboardVM> GetDashboard(IPrincipal User)
        {
            IEnumerable<DashboardVM> dashboard = null;
            if (User != null)
            {
                // if user is internal
                if (HttpContext.Current.User.IsInRole(Key.ROLE_ADMIN) || HttpContext.Current.User.IsInRole(Key.ROLE_STAFF))
                {
                    IEnumerable<DashboardVM> notifications = _context.Notification
                                                            .Where(n => n.Servers.Count() > 0)
                                                            .SelectMany(a => a.Servers
                                                            .Select(s => new DashboardVM()
                                                            {

                                                                SourceReferenceID = s.ReferenceID,
                                                                SourceName = s.ServerName,
                                                                ThreadID = a.ThreadID,
                                                                LevelOfImpact = a.LevelOfImpact.Level,
                                                                ThreadHeading = a.NotificationHeading,
                                                                NotificationType = a.NotificationType.NotificationTypeName,
                                                                SentDateTime = a.SentDateTime,
                                                                Status = a.Status.StatusName

                                                            })).OrderBy(x => x.LevelOfImpact);

                    dashboard = notifications
                                .GroupBy(n => n.ThreadID)
                                .Select(
                                    t => t.OrderByDescending(i => i.SentDateTime).FirstOrDefault()
                    );
                    //to do
                    //tabs that separate incident from maintainance 
                    //heading should show the first not last (from dakota)
                }
                else if (HttpContext.Current.User.IsInRole(Key.ROLE_CLIENT))
                {
                    // if it's external admin
                    var username = User.Identity.Name;
                    var clientID = _context.UserDetail
                                .Where(u => u.User.UserName == username)
                                .FirstOrDefault().ClientID;

                    // get all notifications for all client apps
                    var apps = _context.Client.Where(n => n.ClientID == clientID).SingleOrDefault().Applications;
                    dashboard = GetAppNotifications(dashboard,apps);
                }
                else {
                    // if it's external user
                    var userId = User.Identity.GetUserId();
                    //var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
                    //var userId = userManager.FindByName(username).Id;
                    var apps = _context.UserDetail.Where(u => u.UserID == userId).SingleOrDefault().Applications;
                    dashboard = GetAppNotifications(dashboard, apps);
                }

            }
            return dashboard;
        }
        public IEnumerable<DashboardThreadDetailVM> GetThreadDetails(string threadID)
        {

            IEnumerable<DashboardThreadDetailVM> details = _context.Notification
                                                           .Where(b => b.ThreadID == threadID)
                                                           .Select(c => new DashboardThreadDetailVM
                                                           {
                                                               SentDateTime = c.SentDateTime,
                                                               NotificationHeading = c.NotificationHeading
                                                           });
            return details;
        }

        public IEnumerable<DashboardVM> GetAppNotifications(IEnumerable<DashboardVM> dashboard, ICollection<Application>apps) {
            IEnumerable<DashboardVM> notifications = apps
                                        .SelectMany(a => a.Notifications
                                        .Select(s => new DashboardVM()
                                        {
                                            SourceReferenceID = s.ReferenceID,
                                            SourceName = a.ApplicationName,
                                            ThreadID = s.ThreadID,
                                            LevelOfImpact = s.LevelOfImpact.Level,
                                            ThreadHeading = s.NotificationHeading,
                                            NotificationType = s.NotificationType.NotificationTypeName,
                                            SentDateTime = s.SentDateTime,
                                            Status = s.Status.StatusName
                                        })).OrderBy(x => x.LevelOfImpact);
            dashboard = notifications.GroupBy(n => n.ThreadID)
                            .Select(
                                t => t.OrderByDescending(i => i.SentDateTime).FirstOrDefault()
                            );
            dashboard = dashboard.ToList();
            foreach (var item in dashboard)
            {
                item.ThreadDetail = GetThreadDetails(item.ThreadID);
            }
            return dashboard;
        }
    }
}