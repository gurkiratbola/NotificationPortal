using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NotificationPortal.Models;
using NotificationPortal.Service;
using NotificationPortal.ViewModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using Twilio.Types;

namespace NotificationPortal.Repositories
{
    public class NotificationRepo
    {
        ApplicationDbContext _context = new ApplicationDbContext();
        SelectListRepo _slRepo = new SelectListRepo();

        public NotificationIndexVM CreateIndexModel()
        {
            try
            {
                IEnumerable<NotificationThreadVM> allThreads = GetAllNotifications();

                NotificationIndexVM model = new NotificationIndexVM
                {
                    CurrentFilter = "",
                    CurrentSort = ConstantsRepo.SORT_LEVEL_OF_IMPACT_DESC,
                    IncidentNumberSort = ConstantsRepo.SORT_INCIDENT_NUMBER_ASCE,
                    LevelOfImpactSort = ConstantsRepo.SORT_LEVEL_OF_IMPACT_DESC,
                    NotificationHeadingSort = ConstantsRepo.SORT_NOTIFICATION_BY_HEADING_ASCE,
                    NotificationTypeSort = ConstantsRepo.SORT_NOTIFICATION_BY_TYPE_ASCE,
                    PrioritySort = ConstantsRepo.SORT_NOTIFICATION_BY_PRIORITY_DESC,
                    StatusSort = ConstantsRepo.SORT_STATUS_BY_NAME_DESC,
                    SearchString = "",
                    Page = 1
                };
                model.NotificationTypeList = _slRepo.GetTypeList();
                model.LevelOfImpactList = _slRepo.GetImpactLevelList();
                model.StatusList = _slRepo.GetStatusList(Key.STATUS_TYPE_NOTIFICATION);
                model.PriorityList = _slRepo.GetPriorityList();
                return model;
            }
            catch (Exception)
            {
                // something went while quering the database
                return null;
            }
        }

        public NotificationCreateVM CreateAddModel(NotificationCreateVM model = null)
        {
            if (model == null)
            {
                model = new NotificationCreateVM()
                {
                    IncidentNumber = Guid.NewGuid().ToString(),
                    StartDateTime = DateTime.Now
                };
            }
            model.ApplicationList = GetApplicationList();
            model.ServerList = _slRepo.GetServerList();
            model.NotificationTypeList = _slRepo.GetTypeList();
            model.LevelOfImpactList = _slRepo.GetImpactLevelList();
            model.StatusList = _slRepo.GetStatusList(Key.STATUS_TYPE_NOTIFICATION);
            model.PriorityList = _slRepo.GetPriorityList();
            return model;
        }

        public ThreadDetailVM CreateDetailModel(string incidentNumber)
        {
            try
            {
                IEnumerable<Notification> notifications =
                    _context.Notification.Where(n => n.IncidentNumber == incidentNumber)
                    .OrderBy(n => n.SentDateTime).ToList();

                IEnumerable<NotificationDetailVM> thread =
                    notifications.Select(
                        n => new NotificationDetailVM
                        {
                            ReferenceID = n.ReferenceID,
                            NotificationHeading = n.NotificationHeading,
                            NotificationDescription = n.NotificationDescription,
                            SentDateTime = n.SentDateTime,
                            Status = n.Status.StatusName
                        });

                Notification lastestNotification = notifications.LastOrDefault();

                // TODO: filter servers ??
                IEnumerable<NotificationServerVM> servers =
                    lastestNotification.Servers.Select(
                        s => new NotificationServerVM
                        {
                            ServerName = s.ServerName,
                            ServerType = s.ServerType.ServerTypeName,
                            ServerStatus = s.Status.StatusName,
                            ReferenceID = s.ReferenceID
                        });

                // Get Applications that are associated to this notification
                IEnumerable<Application> associatedNotificationApplications;
                if (HttpContext.Current.User.IsInRole(Key.ROLE_USER))
                {
                    string userId = HttpContext.Current.User.Identity.GetUserId();
                    var userApps = _context.UserDetail
                        .Where(u => u.UserID == userId)
                        .FirstOrDefault().Applications;

                    associatedNotificationApplications = lastestNotification.Applications
                        .Where(a => userApps.Contains(a));
                }
                else if (HttpContext.Current.User.IsInRole(Key.ROLE_CLIENT))
                {
                    string userId = HttpContext.Current.User.Identity.GetUserId();
                    var userApps = _context.UserDetail
                        .Where(u => u.UserID == userId)
                        .FirstOrDefault().Client.Applications;
                    associatedNotificationApplications = lastestNotification.Applications
                        .Where(a => userApps.Contains(a));
                }
                else
                {
                    associatedNotificationApplications = lastestNotification.Applications;
                }

                IEnumerable<NotificationApplicationVM> apps =
                    associatedNotificationApplications.Select(
                        a => new NotificationApplicationVM
                        {
                            ApplicationName = a.ApplicationName,
                            ApplicationURL = a.URL,
                            ApplicationStatus = a.Status.StatusName,
                            ReferenceID = a.ReferenceID
                        }).OrderByDescending(a => a.ApplicationName);

                // If there are no applications associated to this notification
                // get all application associated with server(s)
                // only for admin and staff
                if (apps.Count() == 0
                    && HttpContext.Current.User.IsInRole(Key.ROLE_ADMIN)
                    && HttpContext.Current.User.IsInRole(Key.ROLE_STAFF))
                {
                    apps =
                    lastestNotification.Servers.SelectMany(s => s.Applications.Select(
                        a => new NotificationApplicationVM
                        {
                            ApplicationName = a.ApplicationName,
                            ApplicationURL = a.URL,
                            ApplicationStatus = a.Status.StatusName,
                            ReferenceID = a.ReferenceID
                        })).GroupBy(a => a.ReferenceID)
                        .Select(
                            a => a.FirstOrDefault()
                    ).OrderByDescending(a => a.ApplicationName);
                }

                ThreadDetailVM model = new ThreadDetailVM()
                {
                    IncidentNumber = incidentNumber,
                    // TODO
                    // ApplicationServerName = lastestNotification.Servers.Count == 0 ? lastestNotification.Application.ApplicationName : lastestNotification.Server.ServerName,
                    NotificationType = lastestNotification.NotificationType.NotificationTypeName,
                    LevelOfImpact = lastestNotification.LevelOfImpact.LevelName,
                    Status = lastestNotification.Status.StatusName,
                    StartDateTime = lastestNotification.StartDateTime,
                    EndDateTime = lastestNotification.EndDateTime,
                    Thread = thread,
                    Servers = servers,
                    Applications = apps,
                    Subject = notifications.FirstOrDefault().NotificationHeading,
                    SenderName = notifications.FirstOrDefault().UserDetail.FirstName + " " + notifications.FirstOrDefault().UserDetail.LastName
                };
                return model;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public NotificationCreateVM CreateUpdateModel(string incidentNumber, NotificationCreateVM model = null)
        {
            var lastestNotification =
                _context.Notification.Where(n => n.IncidentNumber == incidentNumber)
                .OrderByDescending(n => n.SentDateTime).FirstOrDefault();

            int headingLength = lastestNotification.NotificationHeading.IndexOf(" (Edit");

            if (model == null)
            {
                model = new NotificationCreateVM()
                {
                    IncidentNumber = incidentNumber,
                    StartDateTime = lastestNotification.StartDateTime,
                    EndDateTime = lastestNotification.EndDateTime,
                    LevelOfImpactID = lastestNotification.LevelOfImpactID,
                    NotificationTypeID = lastestNotification.NotificationTypeID,
                    PriorityID = lastestNotification.PriorityID,
                    StatusID = lastestNotification.StatusID,
                    NotificationDescription = lastestNotification.NotificationDescription,
                    NotificationHeading = lastestNotification.NotificationHeading = headingLength == -1 ?
                        lastestNotification.NotificationHeading :
                        lastestNotification.NotificationHeading.Substring(0, headingLength)

                };
                model.ServerReferenceIDs = lastestNotification.Servers.Select(s => s.ReferenceID).ToArray();
                model.ApplicationReferenceIDs = lastestNotification.Applications.Select(a => a.ReferenceID).ToArray();
            }
            model = CreateAddModel(model);
            return model;
        }

        public NotificationEditVM CreateEditModel(string notificationReferenceID, NotificationEditVM model = null)
        {
            var editingNotification =
                _context.Notification.Where(n => n.ReferenceID == notificationReferenceID)
                .FirstOrDefault();

            if (model == null)
            {
                model = new NotificationEditVM()
                {
                    NotificationReferenceID = notificationReferenceID,
                    IncidentNumber = editingNotification.IncidentNumber,
                    StartDateTime = editingNotification.StartDateTime,
                    EndDateTime = editingNotification.EndDateTime,
                    LevelOfImpactID = editingNotification.LevelOfImpactID,
                    NotificationTypeID = editingNotification.NotificationTypeID,
                    PriorityID = editingNotification.PriorityID,
                    StatusID = editingNotification.StatusID,
                    NotificationDescription = editingNotification.NotificationDescription,
                    NotificationHeading = editingNotification.NotificationHeading
                };
                model.ServerReferenceIDs = editingNotification.Servers.Select(s => s.ReferenceID).ToArray();
                model.ApplicationReferenceIDs = editingNotification.Applications.Select(a => a.ReferenceID).ToArray();
            }
            model.ApplicationList = GetApplicationList();
            model.ServerList = _slRepo.GetServerList();
            model.NotificationTypeList = _slRepo.GetTypeList();
            model.LevelOfImpactList = _slRepo.GetImpactLevelList();
            model.PriorityList = _slRepo.GetPriorityList();
            model.StatusList = _slRepo.GetStatusList(Key.STATUS_TYPE_NOTIFICATION);
            return model;
        }

        public NotificationDetailVM CreateDeleteModel(string notificationReferenceID)
        {
            var deletingNotification =
                _context.Notification.Where(n => n.ReferenceID == notificationReferenceID)
                .FirstOrDefault();
            NotificationDetailVM model = new NotificationDetailVM()
            {
                ReferenceID = notificationReferenceID,
                NotificationDescription = deletingNotification.NotificationDescription,
                NotificationHeading = deletingNotification.NotificationHeading,
                SentDateTime = deletingNotification.SentDateTime,
                IncidentNumber = deletingNotification.IncidentNumber,
                Status = deletingNotification.Status.StatusName
            };
            return model;
        }

        // get all notifications based on roles of current user
        public IEnumerable<NotificationThreadVM> GetAllNotifications()
        {
            try
            {
                IEnumerable<Notification> allNotifications = _context.Notification;
                if (HttpContext.Current.User.IsInRole(Key.ROLE_USER))
                {
                    string userId = HttpContext.Current.User.Identity.GetUserId();
                    var userApps = _context.UserDetail
                        .Where(u => u.UserID == userId)
                        .FirstOrDefault().Applications;
                    allNotifications = userApps
                    .Select(x => new { Application = x, x.Servers })
                    .SelectMany(x => x.Servers
                    .SelectMany(n => n.Notifications
                    .Where(a => a.Applications.Contains(x.Application) || a.Applications.Count() == 0)));
                }
                else if (HttpContext.Current.User.IsInRole(Key.ROLE_CLIENT))
                {
                    string userId = HttpContext.Current.User.Identity.GetUserId();
                    var userApps = _context.UserDetail
                        .Where(u => u.UserID == userId)
                        .FirstOrDefault().Client.Applications;
                    allNotifications = userApps
                    .Select(x => new { Application = x, x.Servers })
                    .SelectMany(x => x.Servers
                    .SelectMany(n => n.Notifications
                    .Where(a => a.Applications.Contains(x.Application) || a.Applications.Count() == 0)));
                }

                IEnumerable<NotificationThreadVM> allThreads = allNotifications
                    .GroupBy(n => n.IncidentNumber)
                    .Select(t => t.OrderBy(i => i.SentDateTime))
                    .Select(
                        t => new NotificationThreadVM()
                        {
                            ReferenceID = t.FirstOrDefault().ReferenceID,
                            IncidentNumber = t.FirstOrDefault().IncidentNumber,
                            NotificationHeading = t.FirstOrDefault().NotificationHeading,
                            SentDateTime = t.FirstOrDefault().SentDateTime,
                            NotificationType = t.LastOrDefault().NotificationType.NotificationTypeName,
                            LevelOfImpact = t.LastOrDefault().LevelOfImpact.LevelName,
                            LevelOfImpactValue = t.LastOrDefault().LevelOfImpact.LevelValue,
                            Priority = t.LastOrDefault().Priority.PriorityName,
                            PriorityValue = t.LastOrDefault().Priority.PriorityValue,
                            Status = t.LastOrDefault().Status.StatusName
                        })
                    .GroupBy(n => n.IncidentNumber)
                    .Select(t => t.OrderByDescending(i => i.SentDateTime).FirstOrDefault());
                return allThreads;
            }
            catch (Exception)
            {
                return null;
            }
        }

        // TODO: implement new API of this method as it is not good with multiselect plugin
        public IEnumerable<ApplicationServerOptionVM> GetApplicationList()
        {
            var apps = _context.Application.Select(a => new { Application = a, Servers = a.Servers });
            var appList = new List<ApplicationServerOptionVM>() { };
            foreach (var app in apps)
            {
                var appServers = app.Servers;
                string serverRefIDs = "";
                if (appServers.Count() > 0)
                {
                    serverRefIDs = string.Join(" ", appServers.Select(i => i.ReferenceID).ToArray());
                }
                appList.Add(new ApplicationServerOptionVM
                {
                    ApplicationName = app.Application.ApplicationName,
                    ReferenceID = app.Application.ReferenceID,
                    ServerReferenceIDs = serverRefIDs
                });
            }
            return appList;
        }

        // create new notification. this is also used for updating a thread with new notification
        public bool CreateNotification(NotificationCreateVM notification, out string msg)
        {

            try
            {
                var userId = HttpContext.Current.User.Identity.GetUserId();
                var sendMethodId = _context.UserDetail
                                .Where(a => a.UserID == userId)
                                .FirstOrDefault().SendMethodID;
                //TO DO: check if it's by server or by Application
                if (notification.ServerReferenceIDs == null)
                {
                    msg = "Must choose a Server";
                    return false;
                }
                if (notification.ApplicationReferenceIDs == null)
                {
                    notification.ApplicationReferenceIDs = new string[0];
                }
                var servers = _context.Server.Where(s => notification.ServerReferenceIDs.Contains(s.ReferenceID));
                var apps = _context.Application.Where(a => notification.ApplicationReferenceIDs.Contains(a.ReferenceID));
                var priorityValue = _context.Notification.Where(n => notification.PriorityID == n.Priority.PriorityID)
                    .Select(n => n.Priority.PriorityValue)
                    .FirstOrDefault();
                Notification newNotification = new Notification()
                {
                    LevelOfImpactID = notification.LevelOfImpactID,
                    NotificationTypeID = notification.NotificationTypeID,
                    NotificationHeading = notification.NotificationHeading,
                    NotificationDescription = notification.NotificationDescription,
                    StatusID = notification.StatusID,
                    PriorityID = notification.PriorityID,
                    UserID = userId,
                    //TO DO: discuss how referenceID is generated
                    ReferenceID = Guid.NewGuid().ToString(),
                    IncidentNumber = notification.IncidentNumber,
                    //TO DO: convert input time to UTC time
                    SentDateTime = DateTime.Now,
                    StartDateTime = notification.StartDateTime,
                    EndDateTime = notification.EndDateTime,
                };
                newNotification.Servers = servers.ToList();
                newNotification.Applications = apps.ToList();

                _context.Notification.Add(newNotification);
                _context.SaveChanges();

                msg = "Notification Sent";
                return true;
            }
            catch (Exception)
            {
                msg = "Notification not created";
                return false;
            }

        }

        // edit a created notification
        public bool EditNotification(NotificationEditVM notification, out string msg)
        {
            try
            {
                //TO DO: check if it's by server or by Application
                if (notification.ServerReferenceIDs == null)
                {
                    msg = "Must choose a Server";
                    return false;
                }
                if (notification.ApplicationReferenceIDs == null)
                {
                    notification.ApplicationReferenceIDs = new string[0];
                }

                var servers = _context.Server.Where(s => notification.ServerReferenceIDs.Contains(s.ReferenceID));
                var apps = _context.Application.Where(a => notification.ApplicationReferenceIDs.Contains(a.ReferenceID));


                var editingNotification = _context.Notification
                        .Where(n => n.ReferenceID == notification.NotificationReferenceID)
                        .FirstOrDefault();

                int headingLength = notification.NotificationHeading.IndexOf(" (Edit");
                editingNotification.NotificationHeading = headingLength == -1 ?
                    notification.NotificationHeading + " (Edited " + DateTime.Now.ToString() + ")" :
                    notification.NotificationHeading.Substring(0, headingLength) + " (Edited " + DateTime.Now.ToString() + ")";
                editingNotification.LevelOfImpactID = notification.LevelOfImpactID;
                editingNotification.NotificationTypeID = notification.NotificationTypeID;
                editingNotification.NotificationDescription = notification.NotificationDescription;
                editingNotification.StatusID = notification.StatusID;
                editingNotification.StartDateTime = notification.StartDateTime;
                editingNotification.EndDateTime = notification.EndDateTime;

                // drop associated servers and apps first
                editingNotification.Servers.Clear();
                editingNotification.Applications.Clear();
                // then set associated servers and apps
                editingNotification.Servers = servers.ToList();
                editingNotification.Applications = apps.ToList();

                _context.SaveChanges();

                msg = "Notification Edited";
                return true;
            }
            catch (Exception ex)
            {
                if (ex is SqlException)
                {

                }
                msg = "Notification not edited";
                return false;
            }

        }

        // delete all notifcations associated to a specific thread
        public bool DeleteThread(string incidentNumber, out string msg)
        {
            try
            {
                var deletingThread = _context.Notification.Where(n => n.IncidentNumber == incidentNumber);
                _context.Notification.RemoveRange(deletingThread);
                _context.SaveChanges();
                msg = "Thread has been deleted";
                return true;
            }
            catch (Exception)
            {
                msg = "Thread has not been deleted";
                return false;
            }

        }

        // delete a single notifcation
        public bool DeleteNotification(string notificationReferenceID, out string msg)
        {
            try
            {
                var deletingNotification = _context.Notification.Where(n => n.ReferenceID == notificationReferenceID).FirstOrDefault();
                _context.Notification.Remove(deletingNotification);
                _context.SaveChanges();
                msg = "Notification has been deleted";
                return true;
            }
            catch (Exception)
            {
                msg = "Notification has not been deleted";
                return false;
            }

        }
        
        // create mails using a template body for users with email as preference recieve method
        public List<MailMessage> CreateMails(NotificationCreateVM notification)
        {
            try
            {
                var servers = _context.Server.Where(s => notification.ServerReferenceIDs.Contains(s.ReferenceID));
                var apps = _context.Application.Where(a => notification.ApplicationReferenceIDs.Contains(a.ReferenceID));
                var priorityValue = _context.Notification.Where(n => notification.PriorityID == n.Priority.PriorityID)
                    .Select(n => n.Priority.PriorityValue)
                    .FirstOrDefault();

                // Get recievers
                List<string> receivers;
                if (apps.Count() == 0)
                {
                    receivers = servers.SelectMany(
                        s => s.Applications.SelectMany(
                            a => a.UserDetails.Where(
                            u => u.SendMethod.SendMethodName == Key.SEND_METHOD_EMAIL
                            || u.SendMethod.SendMethodName == Key.SEND_METHOD_EMAIL_AND_SMS)
                            .Select(u => u.User.Email))).ToList();
                }
                else
                {
                    receivers = apps.SelectMany(
                        a => a.UserDetails.Where(
                            u => u.SendMethod.SendMethodName == Key.SEND_METHOD_EMAIL
                            || u.SendMethod.SendMethodName == Key.SEND_METHOD_EMAIL_AND_SMS)
                            .Select(u => u.User.Email)).ToList();
                }
                receivers = receivers.Distinct().ToList();

                List<MailMessage> mails = new List<MailMessage>();
                foreach (string receiver in receivers)
                {
                    // check if user have confirmed their email
                    UserManager<ApplicationUser> manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
                    ApplicationUser user = manager.FindByEmail(receiver);
                    if (user != null)
                    {
                        if (user.EmailConfirmed)
                        {
                            //create the mail message 
                            MailMessage mail = new MailMessage();

                            //set the addresses 
                            mail.From = new MailAddress("no-reply@notification-portal.com");
                            mail.To.Add(receiver);

                            //set the content 
                            mail.Subject = notification.NotificationHeading;
                            //TODO body needs to be improved
                            mail.Body = TemplateService.NotificationEmail(notification);
                            mail.IsBodyHtml = true;

                            switch (priorityValue)
                            {
                                case Key.PRIORITY_VALUE_HIGH:
                                    mail.Priority = MailPriority.High;
                                    break;
                                case Key.PRIORITY_VALUE_NORMAL:
                                    mail.Priority = MailPriority.Normal;
                                    break;
                                case Key.PRIORITY_VALUE_LOW:
                                    mail.Priority = MailPriority.Low;
                                    break;
                                default:
                                    mail.Priority = MailPriority.Normal;
                                    break;
                            }
                            mails.Add(mail);
                        }
                    }
                }

                return mails;
            }
            catch (Exception)
            {
                return null;
            }
        }

        // get the phone numbers of the users with sms as preference recieve method
        public List<PhoneNumber> GetPhoneNumbers(NotificationCreateVM notification)
        {
            var servers = _context.Server.Where(s => notification.ServerReferenceIDs.Contains(s.ReferenceID));
            var apps = _context.Application.Where(a => notification.ApplicationReferenceIDs.Contains(a.ReferenceID));
            var priorityValue = _context.Notification.Where(n => notification.PriorityID == n.Priority.PriorityID)
                .Select(n => n.Priority.PriorityValue)
                .FirstOrDefault();

            // Get recievers
            List<string> receivers;
            if (apps.Count() == 0)
            {
                receivers = servers.SelectMany(
                    s => s.Applications.SelectMany(
                        a => a.UserDetails.Where(
                            u => u.SendMethod.SendMethodName == Key.SEND_METHOD_SMS
                            || u.SendMethod.SendMethodName == Key.SEND_METHOD_EMAIL_AND_SMS)
                            .Select(u => u.MobilePhone))).ToList();
            }
            else
            {
                receivers = apps.SelectMany(
                    a => a.UserDetails.Where(
                            u => u.SendMethod.SendMethodName == Key.SEND_METHOD_SMS
                            || u.SendMethod.SendMethodName == Key.SEND_METHOD_EMAIL_AND_SMS)
                            .Select(u => u.MobilePhone)).ToList();
            }
            receivers = receivers.Distinct().ToList();

            // Get phone numbers
            List<PhoneNumber> phoneNumbers = new List<PhoneNumber>();
            foreach (var phoneNumber in receivers)
            {
                phoneNumbers.Add(new PhoneNumber(phoneNumber));
            }
            return phoneNumbers;
        }

        // get unique incident # based on notification type
        public string NewIncidentNumber(int notificationTypeID)
        {
            string notificationType = _slRepo.GetTypeList().Where(i => i.Value == notificationTypeID.ToString()).Select(i => i.Text).FirstOrDefault();
            string newIncidentNumber;
            if (notificationType == Key.NOTIFICATION_TYPE_INCIDENT)
            {
                newIncidentNumber = "INC-";
            }
            else if (notificationType == Key.NOTIFICATION_TYPE_MAINTENANCE)
            {
                newIncidentNumber = "MAI-";
            }
            else
            {
                // TODO 
                newIncidentNumber = "UND-";
            }

            int max = 9999999;
            var incidentNumbers = _context.Notification
                .Select(n => n.IncidentNumber).Distinct();

            var incidentNumberSet = new HashSet<int>();
            foreach (var item in incidentNumbers)
            {
                int result = 0;
                string numberString = item.Substring(newIncidentNumber.Length);
                string prefix = item.Substring(0, newIncidentNumber.Length);
                if (prefix == newIncidentNumber && int.TryParse(numberString, out result))
                {
                    incidentNumberSet.Add(result);
                }
            }

            var range = Enumerable.Range(1, max).Where(i => !incidentNumberSet.Contains(i));

            var rand = new Random();
            int index = rand.Next(0, range.Count());
            int randNumber = range.ElementAt(index);

            return newIncidentNumber + randNumber.ToString("D7");
        }
    }
}