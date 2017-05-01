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

        public IEnumerable<DashboardVM> GetAllApplications()
        {
            IEnumerable<DashboardVM> applications = _context.Application.Where(a => a.Status.StatusID == a.StatusID)
                                                    .Select(app => new DashboardVM()
                                                    {
                                                        ApplicationID = app.ApplicationID,
                                                        ApplicationName = app.ApplicationName,
                                                        Description = app.Description,
                                                        URL = app.URL,
                                                        ClientID = app.ClientID,
                                                        ClientName = app.Client.ClientName,
                                                        StatusID = app.Status.StatusID,
                                                        StatusName = app.Status.StatusName
                                                    });

            return applications;
        }
    }
}