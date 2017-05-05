using NotificationPortal.Models;
using NotificationPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace NotificationPortal.Repositories
{
    public class ApplicationRepo
    {
        const string APP_STATUS_TYPE_NAME = "Application";
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        public IEnumerable<ApplicationVM> GetApplicationList()
        {
            IEnumerable<ApplicationVM> applicationList = _context.Application
                                                .Select(c => new ApplicationVM
                                                {
                                                    ApplicationName = c.ApplicationName,
                                                    ReferenceID = c.ReferenceID,
                                                    Description = c.Description,
                                                    URL = c.URL,
                                                    StatusID = c.StatusID,
                                                    ClientID = c.ClientID,
                                                });
            return applicationList;
        }

        public SelectList GetStatusList()
        {
            IEnumerable<SelectListItem> statusList = _context.Status
                                    .Where(a => a.StatusType.StatusTypeName == APP_STATUS_TYPE_NAME)
                                    .Select(sv => new SelectListItem()
                                    {
                                        Value = sv.StatusID.ToString(),
                                        Text = sv.StatusName
                                    });

            return new SelectList(statusList, "Value", "Text");
        }

        public SelectList GetClientList()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            IEnumerable<SelectListItem> clientList = _context.Client
                    .Select(app =>
                                new SelectListItem
                                {
                                    Value = app.ClientID.ToString(),
                                    Text = app.ClientName
                                });

            return new SelectList(clientList, "Value", "Text");
        }


        public bool AddApplication(ApplicationVM application, out string msg)
        {
            Application a = _context.Application.Where(e => e.ApplicationName == application.ApplicationName)
                            .FirstOrDefault();
            if (a != null)
            {
                msg = "Application name already exist.";
                return false;
            }
            try
            {
                Application newApplication = new Application();
                newApplication.ApplicationName = application.ApplicationName;
                newApplication.StatusID = application.StatusID;
                newApplication.ClientID = application.ClientID;
                newApplication.ReferenceID = application.ReferenceID;
                newApplication.Description = application.Description;
                newApplication.URL = application.URL;

                newApplication.ReferenceID = Guid.NewGuid().ToString();
                _context.Application.Add(newApplication);
                _context.SaveChanges();
                msg = "Application successfully added";
                return true;
            }
            catch
            {
                msg = "Failed to add application.";
                return false;
            }
        }

        public ApplicationVM GetApplication(string referenceID)
        {
            ApplicationVM application = _context.Application
                            .Where(a => a.ReferenceID == referenceID)
                            .Select(b => new ApplicationVM
                            {
                                ApplicationName = b.ApplicationName,
                                ReferenceID = b.ReferenceID,
                                Description = b.Description,
                                URL = b.URL,
                                StatusID = b.StatusID,
                                ClientID = b.ClientID,
                            }).FirstOrDefault();
            return application;
        }

        public ApplicationVM GetDeleteApplication(string referenceID)
        {
            ApplicationVM application = _context.Application
                            .Where(a => a.ReferenceID == referenceID)
                            .Select(b => new ApplicationVM
                            {
                                ApplicationName = b.ApplicationName,
                                ReferenceID = b.ReferenceID,
                                Description = b.Description,
                                URL = b.URL,
                                StatusID = b.StatusID,
                                ClientID = b.ClientID,
                            }).FirstOrDefault();
            return application;
        }

        public bool EditApplication(ApplicationVM application, out string msg)
        {
            Application a = _context.Application.Where(b => b.ApplicationName == application.ApplicationName).FirstOrDefault();
            //if (a != null)
            //{
            //    msg = "Application name already exist.";
            //    return false;
            //}
            try
            {
                Application applicationUpdated = _context.Application
                                        .Where(d => d.ReferenceID == application.ReferenceID)
                                        .FirstOrDefault();
                applicationUpdated.ApplicationName = application.ApplicationName;
                applicationUpdated.ReferenceID = application.ReferenceID;
                applicationUpdated.StatusID = application.StatusID;
                applicationUpdated.Description = application.Description;
                applicationUpdated.URL = application.URL;
                applicationUpdated.ClientID = application.ClientID;
                _context.SaveChanges();
                msg = "Application information succesfully updated.";
                return true;
            }
            catch
            {
                msg = "Failed to update application.";
                return false;
            }
        }

        public bool DeleteApplication(string referenceID, out string msg)
        {
            // check if applications exists
            Application applicationToBeDeleted = _context.Application
                                    .Where(a => a.ReferenceID == referenceID)
                                    .FirstOrDefault();
            // check applications associated with Server
            var applicationServers = _context.Server
                                       .Where(a => a.ReferenceID == referenceID)
                                       .FirstOrDefault();

            // check applications associated with Client
            var applicationUsers = _context.UserDetail
                                       .Where(a => a.ReferenceID == referenceID)
                                       .FirstOrDefault();

            var applicationNotifications = _context.Notification
                                     .Where(a => a.ReferenceID == referenceID)
                                     .FirstOrDefault();
            if (applicationToBeDeleted == null)
            {
                msg = "Client could not be deleted.";
                return false;
            }
            if (applicationServers != null)
            {
                msg = "Application has server(s) associated, cannot be deleted";
                return false;
            }

            if (applicationUsers != null)
            {
                msg = "Application has a users associated, cannot be deleted";
                return false;
            }

            if (applicationNotifications != null)
            {
                msg = "Application has a Notifications associated, cannot be deleted";
                return false;
            }

            try
            {
                _context.Application.Remove(applicationToBeDeleted);
                _context.SaveChanges();
                msg = "Client Successfully Deleted";
                return true;
            }
            catch
            {
                msg = "Failed to update client.";
                return false;
            }

        }
    }
}
    

