using NotificationPortal.Models;
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
        private IEnumerable<ApplicationVM> GetApplicaitons()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            IEnumerable<ApplicationVM> appList = db.Application
                    .Select(app => new ApplicationVM()
                    {
                        ApplicationID = app.ApplicationID,
                        ApplicationName = app.ApplicationName,
                        ClientID = app.ClientID,
                        Description = app.Description,
                        //StatusID = app.StatusID,
                        URL = app.URL
                    });
            return appList;
        }

        public IEnumerable<SelectListItem> GetApplicaitonList()
        {
            var list = GetApplicaitons();
            return new SelectList(list, "Value", "Text");
        }

        private IEnumerable<ServerVM> GetServers()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            IEnumerable<ServerVM> serverList = db.Server
                    .Select(sv => new ServerVM()
                    {
                        ServerID= sv.ServerID,
                        ServerName = sv.ServerName,
                        ServerStatusID = sv.StatusID,
                    });

            return serverList;
        }

        public IEnumerable<SelectListItem> GetServerList()
        {
            var list = GetServers();
            return new SelectList(list, "Value", "Text");
        }

        private IEnumerable<NotificationTypeVM> GetTypes()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            IEnumerable<NotificationTypeVM> typeList = db.NotificationType
                    .Select(type => new NotificationTypeVM()
                    {
                        NotificationTypeID = type.NotificationTypeID,
                        NotificationTypeName = type.NotificationTypeName
                    });

            return typeList;
        }

        public IEnumerable<SelectListItem> GetTypeList()
        {
            var list = GetTypes();
            return new SelectList(list, "Value", "Text");
        }

        private IEnumerable<LevelOfImpactVM> GetImpactLevels()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            IEnumerable<LevelOfImpactVM> impactList = db.LevelOfImpact
                    .Select(impact => new LevelOfImpactVM()
                    {
                        LevelOfImpactID = impact.LevelOfImpactID,
                        Type = impact.Type
                    });

            return impactList;
        }

        public IEnumerable<SelectListItem> GetImpactLevelList()
        {
            var list = GetImpactLevels();
            return new SelectList(list, "Value", "Text");
        }

        private IEnumerable<StatusVM> GetNotificationStatus()
        {
            const int NOTIFICATION_STATUS_INDEX = 3;
            ApplicationDbContext db = new ApplicationDbContext();
            IEnumerable<StatusVM> statusList = db.Status
                    .Where(a=>a.StatusTypeID == NOTIFICATION_STATUS_INDEX)
                    .Select(status => new StatusVM()
                    {
                           StatusID = status.StatusID,
                           StatusName = status.StatusName
                    });

            return statusList;
        }

        public IEnumerable<SelectListItem> GetNotificationSatusList()
        {
            var list = GetNotificationStatus();
            return new SelectList(list, "Value", "Text");
        }
    }
}