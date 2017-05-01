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
        ApplicationDbContext db = new ApplicationDbContext();
        public SelectList GetApplicaitonList()
        {
            IEnumerable<SelectListItem> appList = db.Application
                    .Select(app => 
                                new SelectListItem
                                {
                                    Value = app.ApplicationID.ToString(),
                                    Text = app.ApplicationName
                                });

            return new SelectList(appList, "Value", "Text");
        }

        public SelectList GetServerList()
        {
            IEnumerable<SelectListItem> serverList = db.Server
                    .Select(sv => new SelectListItem()
                    {
                        Value = sv.ServerID.ToString(),
                        Text = sv.ServerName
                    });

            return new SelectList(serverList, "Value", "Text");
        }

        public SelectList GetTypeList()
        {
            IEnumerable<SelectListItem> typeList = db.NotificationType
                    .Select(type => new SelectListItem()
                    {
                        Value = type.NotificationTypeID.ToString(),
                        Text = type.NotificationTypeName
                    });

            return new SelectList(typeList, "Value", "Text");
        }

        public SelectList GetImpactLevelList()
        {
            IEnumerable<SelectListItem> impactList = db.LevelOfImpact
                    .Select(impact => new SelectListItem()
                    {
                        Value = impact.LevelOfImpactID.ToString(),
                        Text = impact.Level
                    });

            return new SelectList(impactList, "Value", "Text");
        }

        public SelectList GetNotificationSatusList()
        {
            const int NOTIFICATION_STATUS_INDEX = 23;
            IEnumerable<SelectListItem> statusList = db.Status
                    .Where(a => a.StatusTypeID == NOTIFICATION_STATUS_INDEX)
                    .Select(status => new SelectListItem()
                    {
                        Value = status.StatusID.ToString(),
                        Text = status.StatusName
                    });

            return new SelectList(statusList, "Value", "Text");
        }
        
        public SelectList GetSendMethodList()
        {
            IEnumerable<SelectListItem> sendMethodList = db.SendMethod
                    .Select(sendMethod => new SelectListItem()
                    {
                        Value = sendMethod.SendMethodID.ToString(),
                        Text = sendMethod.SendMethodName
                    });

            return new SelectList(sendMethodList, "Value", "Text");
        }

        public IEnumerable<NotificationIndexVM> GetAllNotifications()
        {

            try
            {
                ApplicationDbContext db = new ApplicationDbContext();
                return db.Notification.Select(
                    n=>new NotificationIndexVM(){
                        ThreadID = n.ThreadID,
                        Source = n.Server.ServerName == null ? "Application":"Server",
                        ApplicationServerName = n.Server.ServerName == null ? n.Application.ApplicationName:n.Server.ServerName,
                        NotificationType = n.NotificationType.NotificationTypeName,
                        LevelOfImpact = n.LevelOfImpact.Level,
                        NotificationHeading = n.NotificationHeading,
                        Status = n.Status.StatusName,
                        SentDateTime = n.SentDateTime,
                        StartDateTime = n.StartDateTime,
                        EndDateTime = n.EndDateTime,
                        Client = n.Application.Client.ClientName
                    }
                );
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public NotificationCreateVM createAddModel(NotificationCreateVM model = null)
        {
            if (model == null)
            {
                model = new NotificationCreateVM();
            }
            model.ApplicationList = GetApplicaitonList();
            model.SendMethodList = GetSendMethodList();
            model.ServerList = GetServerList();
            model.NotificationTypeList = GetTypeList();
            model.LevelOfImpactList = GetImpactLevelList();
            model.StatusList = GetNotificationSatusList();
            return model;
        }
        public bool CreateNotification(NotificationCreateVM notification, out string msg)
        {

            try
            {
                if(notification.ThreadID == null){
                    notification.ThreadID = NewThreadID();
                }
                if(notification.Source == "Application")
                {
                    notification.ServerID = null;
                }
                else
                {
                    notification.ApplicationID = null;
                }
                Notification newNotification = new Notification()
                {
                    LevelOfImpactID = notification.LevelOfImpactID,
                    NotificationTypeID = notification.NotificationTypeID,
                    NotificationHeading = notification.NotificationHeading,
                    NotificationDescription = notification.NotificationDescription,
                    StatusID = notification.StatusID,
                    SendMethodID = notification.SentMethodID,
                    ServerID = notification.ServerID,
                    ApplicationID = notification.ApplicationID,
                    //TO DO: discuss how referenceID is generated
                    ReferenceID = Guid.NewGuid().ToString(),
                    ThreadID = (int) notification.ThreadID,
                    //TO DO: convert input time to UTC time
                    SentDateTime = DateTime.Now,
                    StartDateTime = notification.StartDateTime,
                    EndDateTime = notification.EndDateTime,
                };


                //TO DO: check if it's by server or by Application
                if (notification.ServerID==null && notification.ApplicationID==null)
                {
                    msg = "Must choose a Application or Server";
                    return false;
                }

                db.Notification.Add(newNotification);
                db.SaveChanges();
                //TO DO: send the emails here, use levelOfImpactID here

                msg = "Notification Sent";
                return true;
            } catch (Exception ex){
                msg = "Notification cannot be created";
                return false;
            }

        }

        public int NewThreadID()
        {
            var lastThreadID = db.Notification.OrderByDescending(n => n.ThreadID).FirstOrDefault().ThreadID;
            return lastThreadID + 1;
        }

        public NotificationDetailVM createDetailModel(int threadID)
        {
            IEnumerable<Notification> notifications =
                db.Notification.Where(n => n.ThreadID == threadID)
                .OrderBy(n => n.SentDateTime);

            IEnumerable<NotificationSpecificDetailVM> thread =
                notifications.Select(
                    n=> new NotificationSpecificDetailVM
                    {
                        NotificationHeading = n.NotificationHeading,
                        NotificationDescription = n.NotificationDescription,
                        SentDateTime = n.SentDateTime
                    });

            Notification lastestNotification = notifications.LastOrDefault();
            NotificationDetailVM model = new NotificationDetailVM()
            {
                Source = lastestNotification.Server.ServerName == null ? "Application" : "Server",
                ApplicationServerName = lastestNotification.Server.ServerName == null ? lastestNotification.Application.ApplicationName : lastestNotification.Server.ServerName,
                NotificationType = lastestNotification.NotificationType.NotificationTypeName,
                LevelOfImpact = lastestNotification.LevelOfImpact.Level,
                Status = lastestNotification.Status.StatusName,
                StartDateTime = lastestNotification.StartDateTime,
                EndDateTime = lastestNotification.EndDateTime,
                // TODO: query list of Clients
                Client = lastestNotification.Application.Client.ClientName,
                Thread = thread
            };
            return model;
        }
    }
}

//parse timezone

//int offsetNum = 0;
//int.TryParse(timeOffsetString, out offsetNum);