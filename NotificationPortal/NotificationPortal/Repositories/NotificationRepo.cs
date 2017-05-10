using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NotificationPortal.Models;
using NotificationPortal.ViewModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace NotificationPortal.Repositories
{
    public class NotificationRepo
    {
        ApplicationDbContext _context = new ApplicationDbContext();
        SelectListRepo _slRepo = new SelectListRepo();

        public NotificationIndexVM CreateIndexModel(string sortOrder, string currentFilter, string searchString, int? page, NotificationIndexVM model = null)
        {
            IEnumerable<NotificationThreadVM> allThreads = model == null ? GetAllNotifications() : GetFilteredNotifications(model);

            // build the index model based on sort/filter/page information
            page = searchString == null ? page : 1;
            searchString = searchString ?? currentFilter;
            int pageNumber = (page ?? 1);

            model = new NotificationIndexVM
            {
                Threads = Sort(allThreads, sortOrder, searchString).ToPagedList(pageNumber, 3),
                CurrentFilter = searchString,
                CurrentSort = sortOrder,
                LevelOfImpactSort = sortOrder == ConstantsRepo.SORT_LEVEL_OF_IMPACT_DESC ? ConstantsRepo.SORT_LEVEL_OF_IMPACT_ASCE : ConstantsRepo.SORT_LEVEL_OF_IMPACT_DESC,
                NotificationHeadingSort = sortOrder == ConstantsRepo.SORT_NOTIFICATION_BY_HEADING_DESC ? ConstantsRepo.SORT_NOTIFICATION_BY_HEADING_ASCE : ConstantsRepo.SORT_NOTIFICATION_BY_HEADING_DESC,
                NotificationTypeSort = sortOrder == ConstantsRepo.SORT_NOTIFICATION_BY_TYPE_DESC ? ConstantsRepo.SORT_NOTIFICATION_BY_TYPE_ASCE : ConstantsRepo.SORT_NOTIFICATION_BY_TYPE_DESC,
                PrioritySort = sortOrder == ConstantsRepo.SORT_NOTIFICATION_BY_PRIORITY_DESC ? ConstantsRepo.SORT_NOTIFICATION_BY_PRIORITY_ASCE : ConstantsRepo.SORT_NOTIFICATION_BY_PRIORITY_DESC,
                StatusSort = sortOrder == ConstantsRepo.SORT_STATUS_BY_NAME_DESC ? ConstantsRepo.SORT_STATUS_BY_NAME_ASCE : ConstantsRepo.SORT_STATUS_BY_NAME_DESC,
                SearchString = "",
                Page = pageNumber
            };
            model.NotificationTypeList = _slRepo.GetTypeList();
            model.LevelOfImpactList = _slRepo.GetImpactLevelList();
            model.StatusList = _slRepo.GetStatusList(Key.STATUS_TYPE_NOTIFICATION);
            model.PriorityList = _slRepo.GetPriorityList();
            return model;
        }

        public NotificationCreateVM CreateAddModel(NotificationCreateVM model = null)
        {
            if (model == null)
            {
                model = new NotificationCreateVM()
                {
                    IncidentNumber = Guid.NewGuid().ToString()
                };
            }
            model.ApplicationList = GetApplicationList();
            model.SendMethodList = _slRepo.GetSendMethodList();
            model.ServerList = _slRepo.GetServerList();
            model.NotificationTypeList = _slRepo.GetTypeList();
            model.LevelOfImpactList = _slRepo.GetImpactLevelList();
            model.StatusList = _slRepo.GetStatusList(Key.STATUS_TYPE_NOTIFICATION);
            model.ProirityList = _slRepo.GetPriorityList();
            return model;
        }

        public ThreadDetailVM CreateDetailModel(string incidentNumber)
        {
            IEnumerable<Notification> notifications =
                _context.Notification.Where(n => n.IncidentNumber == incidentNumber)
                .OrderBy(n => n.SentDateTime);

            IEnumerable<NotificationDetailVM> thread =
                notifications.Select(
                    n => new NotificationDetailVM
                    {
                        ReferenceID = n.ReferenceID,
                        NotificationHeading = n.NotificationHeading,
                        NotificationDescription = n.NotificationDescription,
                        SentDateTime = n.SentDateTime
                    });

            Notification lastestNotification = notifications.LastOrDefault();

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
            IEnumerable<NotificationApplicationVM> apps =
                lastestNotification.Applications.Select(
                    a => new NotificationApplicationVM
                    {
                        ApplicationName = a.ApplicationName,
                        ApplicationURL = a.URL,
                        ApplicationStatus = a.Status.StatusName,
                        ReferenceID = a.ReferenceID
                    }).OrderByDescending(a => a.ApplicationName);

            // If there are no applications associated to this notification
            // get all application associated with server(s)
            if (apps.Count() == 0)
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
                Applications = apps
            };
            return model;
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
                    SentMethodID = lastestNotification.SendMethodID,
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
                    SentMethodID = editingNotification.SendMethodID,
                    StatusID = editingNotification.StatusID,
                    NotificationDescription = editingNotification.NotificationDescription,
                    NotificationHeading = editingNotification.NotificationHeading
                };
                model.ServerReferenceIDs = editingNotification.Servers.Select(s => s.ReferenceID).ToArray();
                model.ApplicationReferenceIDs = editingNotification.Applications.Select(a => a.ReferenceID).ToArray();
            }
            model.ApplicationList = GetApplicationList();
            model.SendMethodList = _slRepo.GetSendMethodList();
            model.ServerList = _slRepo.GetServerList();
            model.NotificationTypeList = _slRepo.GetTypeList();
            model.LevelOfImpactList = _slRepo.GetImpactLevelList();
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
                IncidentNumber = deletingNotification.IncidentNumber
            };
            return model;
        }

        public IEnumerable<NotificationThreadVM> GetAllNotifications()
        {
            try
            {
                IEnumerable<Notification> allNotifications = _context.Notification;
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
                            Priority = t.LastOrDefault().Priority.PriorityName,
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

        public IEnumerable<NotificationThreadVM> GetFilteredNotifications(NotificationIndexVM model)
        {
            try
            {
                model.NotificationTypeIDs = model.NotificationTypeIDs ?? _slRepo.GetTypeList().Select(o => int.Parse(o.Value)).ToArray();
                model.LevelOfImpactIDs = model.LevelOfImpactIDs ?? _slRepo.GetImpactLevelList().Select(o => int.Parse(o.Value)).ToArray();
                model.PriorityIDs = model.PriorityIDs ?? _slRepo.GetPriorityList().Select(o => int.Parse(o.Value)).ToArray();
                model.StatusIDs = model.StatusIDs ?? _slRepo.GetStatusList(Key.STATUS_TYPE_NOTIFICATION).Select(o => int.Parse(o.Value)).ToArray();
                IEnumerable<Notification> allNotifications = _context.Notification;
                IEnumerable<NotificationThreadVM> allThreads = allNotifications
                    .GroupBy(n => n.IncidentNumber)
                    .Select(t => t.OrderBy(i => i.SentDateTime))
                    .Select(n => new { First = n.FirstOrDefault(), Last = n.LastOrDefault() })
                    .Where(t => model.NotificationTypeIDs.Contains(t.Last.NotificationTypeID)
                    && model.LevelOfImpactIDs.Contains(t.Last.LevelOfImpactID)
                    && model.StatusIDs.Contains(t.Last.StatusID)
                    && model.PriorityIDs.Contains(t.Last.PriorityID))
                    .Select(
                        t => new NotificationThreadVM()
                        {
                            ReferenceID = t.First.ReferenceID,
                            IncidentNumber = t.First.IncidentNumber,
                            NotificationHeading = t.First.NotificationHeading,
                            SentDateTime = t.First.SentDateTime,
                            NotificationType = t.Last.NotificationType.NotificationTypeName,
                            LevelOfImpact = t.Last.LevelOfImpact.LevelName,
                            Priority = t.Last.Priority.PriorityName,
                            Status = t.Last.Status.StatusName
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

        public bool CreateNotification(NotificationCreateVM notification, out string msg)
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
                var priorityValue = _context.Notification.Where(n => notification.ProirityID == n.Priority.PriorityID)
                    .Select(n => n.Priority.PriorityValue)
                    .FirstOrDefault();
                Notification newNotification = new Notification()
                {
                    LevelOfImpactID = notification.LevelOfImpactID,
                    NotificationTypeID = notification.NotificationTypeID,
                    NotificationHeading = notification.NotificationHeading,
                    NotificationDescription = notification.NotificationDescription,
                    StatusID = notification.StatusID,
                    PriorityID = notification.ProirityID,
                    SendMethodID = notification.SentMethodID,
                    //TO DO: discuss how referenceID is generated
                    ReferenceID = Guid.NewGuid().ToString(),
                    IncidentNumber = notification.IncidentNumber ?? Guid.NewGuid().ToString(),
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
                editingNotification.SendMethodID = notification.SentMethodID;
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

        public IEnumerable<NotificationThreadVM> Sort(IEnumerable<NotificationThreadVM> list, string sortOrder, string searchString)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                list = list.Where(n => n.NotificationHeading.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case ConstantsRepo.SORT_LEVEL_OF_IMPACT_ASCE:
                    // TODO: modify to severity value if implement in table later
                    list = list.OrderBy(n => n.LevelOfImpact);
                    break;

                case ConstantsRepo.SORT_NOTIFICATION_BY_HEADING_ASCE:
                    list = list.OrderBy(n => n.NotificationHeading);
                    break;

                case ConstantsRepo.SORT_NOTIFICATION_BY_HEADING_DESC:
                    list = list.OrderByDescending(n => n.NotificationHeading);
                    break;

                case ConstantsRepo.SORT_NOTIFICATION_BY_TYPE_ASCE:
                    list = list.OrderBy(n => n.NotificationType);
                    break;

                case ConstantsRepo.SORT_NOTIFICATION_BY_TYPE_DESC:
                    list = list.OrderByDescending(n => n.NotificationType);
                    break;

                case ConstantsRepo.SORT_NOTIFICATION_BY_PRIORITY_ASCE:
                    list = list.OrderBy(n => n.Priority);
                    break;

                case ConstantsRepo.SORT_NOTIFICATION_BY_PRIORITY_DESC:
                    list = list.OrderByDescending(n => n.Priority);
                    break;
                    
                case ConstantsRepo.SORT_STATUS_BY_NAME_ASCE:
                    list = list.OrderBy(n => n.Status);
                    break;

                case ConstantsRepo.SORT_STATUS_BY_NAME_DESC:
                    list = list.OrderByDescending(n => n.Status);
                    break;

                default:
                    // TODO: modify to severity value if implement in table later
                    list = list.OrderByDescending(n => n.LevelOfImpact);
                    break;
            }
            return list;
        }

        public List<MailMessage> CreateMails(NotificationCreateVM notification)
        {
            try
            {
                var servers = _context.Server.Where(s => notification.ServerReferenceIDs.Contains(s.ReferenceID));
                var apps = _context.Application.Where(a => notification.ApplicationReferenceIDs.Contains(a.ReferenceID));
                var priorityValue = _context.Notification.Where(n => notification.ProirityID == n.Priority.PriorityID)
                    .Select(n => n.Priority.PriorityValue)
                    .FirstOrDefault();

                // Get recievers
                List<string> receivers;
                if (apps.Count() == 0)
                {
                    receivers = servers.SelectMany(s => s.Applications.SelectMany(a => a.UserDetails.Select(u => u.User.Email))).ToList();
                }
                else
                {
                    receivers = apps.SelectMany(a => a.UserDetails.Select(u => u.User.Email)).ToList();
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
                            mail.Body = notification.NotificationDescription;
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

        // TODO verify the functionality
        public string NewIncidentNumber(string notificationType)
        {
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
                newIncidentNumber = "UND-";
            }

            int max = 20;
            var incidentNumbers = _context.Notification
                .Where(n => n.IncidentNumber.Substring(0, newIncidentNumber.Length) == newIncidentNumber)
                .Select(n => int.Parse(n.IncidentNumber.Substring(newIncidentNumber.Length)));
            var incidentNumberSet = new HashSet<int>(incidentNumbers);

            var range = Enumerable.Range(1, max).Where(i => !incidentNumberSet.Contains(i));

            var rand = new Random();
            int index = rand.Next(0, max - incidentNumberSet.Count);
            int randNumber = range.ElementAt(index);

            return newIncidentNumber + randNumber.ToString();
        }
    }
}

//parse timezone

//int offsetNum = 0;
//int.TryParse(timeOffsetString, out offsetNum);