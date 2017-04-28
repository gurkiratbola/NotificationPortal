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
        public IEnumerable<SelectListItem> GetApplicaitonList()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            IEnumerable<SelectListItem> appList = db.Application
                    .Select(app => 
                                new SelectListItem
                                {
                                    Value = app.ApplicationID.ToString(),
                                    Text = app.ApplicationName
                                });

            return new SelectList(appList, "Value", "Text");
        }

        public IEnumerable<SelectListItem> GetServerList()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            IEnumerable<SelectListItem> serverList = db.Server
                    .Select(sv => new SelectListItem()
                    {
                        Value = sv.ServerID.ToString(),
                        Text = sv.ServerName
                    });

            return new SelectList(serverList, "Value", "Text");
        }

        public IEnumerable<SelectListItem> GetTypeList()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            IEnumerable<SelectListItem> typeList = db.NotificationType
                    .Select(type => new SelectListItem()
                    {
                        Value = type.NotificationTypeID.ToString(),
                        Text = type.NotificationTypeName
                    });

            return new SelectList(typeList, "Value", "Text");
        }

        public IEnumerable<SelectListItem> GetImpactLevelList()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            IEnumerable<SelectListItem> impactList = db.LevelOfImpact
                    .Select(impact => new SelectListItem()
                    {
                        Value = impact.LevelOfImpactID.ToString(),
                        Text = impact.Type
                    });

            return new SelectList(impactList, "Value", "Text");
        }

        public IEnumerable<SelectListItem> GetNotificationSatusList()
        {
            const int NOTIFICATION_STATUS_INDEX = 3;
            ApplicationDbContext db = new ApplicationDbContext();
            IEnumerable<SelectListItem> statusList = db.Status
                    .Where(a=>a.StatusTypeID == NOTIFICATION_STATUS_INDEX)
                    .Select(status => new SelectListItem()
                    {
                        Value = status.StatusID.ToString(),
                        Text = status.StatusName
                    });

            return new SelectList(statusList, "Value", "Text");
        }

    }
}