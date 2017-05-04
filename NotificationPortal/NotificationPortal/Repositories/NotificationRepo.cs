using NotificationPortal.Models;
using NotificationPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static NotificationPortal.ViewModels.ValidationVM;

namespace NotificationPortal.Repositories
{
    public class NotificationRepo
    {
        ApplicationDbContext _context = new ApplicationDbContext();
        SelectListRepo _slRepo = new SelectListRepo();

        public IEnumerable<NotificationIndexVM> GetAllNotifications()
        {
            try
            {
                IEnumerable<NotificationIndexVM> allNotifications = _context.Notification.Select(
                    n => new NotificationIndexVM()
                    {
                        ThreadID = n.ThreadID,
                        ReferenceID = n.ReferenceID,
                        NotificationType = n.NotificationType.NotificationTypeName,
                        LevelOfImpact = n.LevelOfImpact.Level,
                        NotificationHeading = n.NotificationHeading,
                        Status = n.Status.StatusName,
                        SentDateTime = n.SentDateTime
                    }
                );


                IEnumerable<NotificationIndexVM> allThreads = allNotifications
                    .GroupBy(n => n.ThreadID)
                    .Select(
                        t => t.OrderByDescending(i => i.SentDateTime).FirstOrDefault()
                );
                return allThreads;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public NotificationCreateVM CreateAddModel(NotificationCreateVM model = null)
        {
            if (model == null)
            {
                model = new NotificationCreateVM();
                model.ThreadID = Guid.NewGuid().ToString();
            }
            model.ApplicationList = GetApplicationList();
            model.SendMethodList = _slRepo.GetSendMethodList();
            model.ServerList = _slRepo.GetServerList();
            model.NotificationTypeList = _slRepo.GetTypeList();
            model.LevelOfImpactList = _slRepo.GetImpactLevelList();
            model.StatusList = _slRepo.GetStatusList(Key.STATUS_TYPE_NOTIFICATION);
            return model;
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
                Notification newNotification = new Notification()
                {
                    LevelOfImpactID = notification.LevelOfImpactID,
                    NotificationTypeID = notification.NotificationTypeID,
                    NotificationHeading = notification.NotificationHeading,
                    NotificationDescription = notification.NotificationDescription,
                    StatusID = notification.StatusID,
                    SendMethodID = notification.SentMethodID,
                    //TO DO: discuss how referenceID is generated
                    ReferenceID = Guid.NewGuid().ToString(),
                    ThreadID = notification.ThreadID ?? Guid.NewGuid().ToString(),
                    //TO DO: convert input time to UTC time
                    SentDateTime = DateTime.Now,
                    StartDateTime = notification.StartDateTime,
                    EndDateTime = notification.EndDateTime,
                };
                newNotification.Servers = servers.ToList();
                newNotification.Applications = apps.ToList();

                _context.Notification.Add(newNotification);
                _context.SaveChanges();
                //TO DO: send the emails here, use levelOfImpactID here

                msg = "Notification Sent";
                return true;
            } catch (Exception ex){
                msg = "Notification not created";
                return false;
            }

        }

        public NotificationDetailVM CreateDetailModel(string threadID)
        {
            IEnumerable<Notification> notifications =
                _context.Notification.Where(n => n.ThreadID == threadID)
                .OrderBy(n => n.SentDateTime);

            IEnumerable<NotificationSpecificDetailVM> thread =
                notifications.Select(
                    n=> new NotificationSpecificDetailVM
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
            if (apps.Count()==0){
                apps =
                lastestNotification.Servers.SelectMany(s=>s.Applications.Select(
                    a => new NotificationApplicationVM
                    {
                        ApplicationName = a.ApplicationName,
                        ApplicationURL = a.URL,
                        ApplicationStatus = a.Status.StatusName,
                        ReferenceID  = a.ReferenceID
                    })).GroupBy(a => a.ReferenceID)
                    .Select(
                        a => a.FirstOrDefault()
                ).OrderByDescending(a => a.ApplicationName);
            }

            NotificationDetailVM model = new NotificationDetailVM()
            {
                ThreadID = threadID,
                Source = lastestNotification.Servers.Count == 0 ? "Application" : "Server",
                // TODO
                // ApplicationServerName = lastestNotification.Servers.Count == 0 ? lastestNotification.Application.ApplicationName : lastestNotification.Server.ServerName,
                NotificationType = lastestNotification.NotificationType.NotificationTypeName,
                LevelOfImpact = lastestNotification.LevelOfImpact.Level,
                Status = lastestNotification.Status.StatusName,
                StartDateTime = lastestNotification.StartDateTime,
                EndDateTime = lastestNotification.EndDateTime,
                Thread = thread,
                Servers = servers,
                Applications = apps
            };
            return model;
        }

        public NotificationCreateVM CreateUpdateModel(string threadID, NotificationCreateVM model = null)
        {
            var lastestNotification = 
                _context.Notification.Where(n => n.ThreadID == threadID)
                .OrderByDescending(n => n.SentDateTime).FirstOrDefault();

            if (model == null)
            {
                model = new NotificationCreateVM() {
                    ThreadID = threadID,
                    StartDateTime = lastestNotification.StartDateTime,
                    EndDateTime = lastestNotification.EndDateTime,
                    Source = lastestNotification.Applications == null ? "Application":"Server",
                    LevelOfImpactID = lastestNotification.LevelOfImpactID,
                    NotificationTypeID = lastestNotification.NotificationTypeID,
                    SentMethodID = lastestNotification.SendMethodID,
                    StatusID = lastestNotification.StatusID,
                    NotificationDescription = lastestNotification.NotificationDescription,
                    NotificationHeading = lastestNotification.NotificationHeading
                };
                model.ServerReferenceIDs = lastestNotification.Servers.Select(s => s.ReferenceID).ToArray();
                model.ApplicationReferenceIDs = lastestNotification.Applications.Select(a => a.ReferenceID).ToArray();
            }
            model = CreateAddModel(model);
            return model;
        }

        public IEnumerable<ApplicationOptionVM> GetApplicationList()
        {
            //var apps = _context.Server.Select(s=>s.Applications.Select(
            //    a => new { AppName = a.ApplicationName, AppRef = a.ReferenceID }));
            //var group = apps.GroupBy(a=>a.AppRef);
            //var grouped = group.Select(g=>new ApplicationOptionVM
            //{
            //    ApplicationName = g.First().AppName,
            //    ReferenceID = g.First().AppRef,
            //    //ServerReferenceIDs = string.Join(" ", g.Select(i=>i.ServerRef))
            //});
            return new List<ApplicationOptionVM>() {new ApplicationOptionVM()};
        }
    }
}

//string.Join(" ", a.Servers.Select(s => s.ReferenceID).ToArray())
//parse timezone

//int offsetNum = 0;
//int.TryParse(timeOffsetString, out offsetNum);