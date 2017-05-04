using NotificationPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NotificationPortal.Repositories
{
    public class SelectListRepo
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        public IEnumerable<SelectListItem> GetClientList()
        {
            List<SelectListItem> clientList = _context.Client.Select(app => new SelectListItem
                                              {
                                                  Value = app.ClientID.ToString(),
                                                  Text = app.ClientName
                                              }).ToList();

            clientList.Add(new SelectListItem { Value = "-1", Text = "" });
            //clientList.OrderByDescending(x => x.Value);

            return new SelectList(clientList, "Value", "Text");
        }

        public SelectList GetStatusList(string statusType)
        {
            IEnumerable<SelectListItem> statusList = _context.Status.Where(s => s.StatusType.StatusTypeName == statusType)
                                                     .Select(s => new SelectListItem()
                                                     {
                                                         Value = s.StatusID.ToString(),
                                                         Text = s.StatusName
                                                     });

            return new SelectList(statusList, "Value", "Text");
        }

        public SelectList GetApplicationList()
        {
            IEnumerable<SelectListItem> appList = _context.Application.Select(app =>
                                                  new SelectListItem
                                                  {
                                                      Value = app.ApplicationID.ToString(),
                                                      Text = app.ApplicationName
                                                  });

            return new SelectList(appList, "Value", "Text");
        }

        public SelectList GetServerList()
        {
            IEnumerable<SelectListItem> serverList = _context.Server.Select(sv => new SelectListItem()
                                                     {
                                                         Value = sv.ServerID.ToString(),
                                                         Text = sv.ServerName
                                                     });

            return new SelectList(serverList, "Value", "Text");
        }

        public SelectList GetTypeList()
        {
            IEnumerable<SelectListItem> typeList = _context.NotificationType.Select(type => new SelectListItem()
                                                   {
                                                       Value = type.NotificationTypeID.ToString(),
                                                       Text = type.NotificationTypeName
                                                   });

            return new SelectList(typeList, "Value", "Text");
        }

        public SelectList GetImpactLevelList()
        {
            IEnumerable<SelectListItem> impactList = _context.LevelOfImpact.Select(impact => new SelectListItem()
                                                     {
                                                         Value = impact.LevelOfImpactID.ToString(),
                                                         Text = impact.Level
                                                     });

            return new SelectList(impactList, "Value", "Text");
        }

        public SelectList GetSendMethodList()
        {
            IEnumerable<SelectListItem> sendMethodList = _context.SendMethod.Select(sendMethod => new SelectListItem()
                                                         {
                                                             Value = sendMethod.SendMethodID.ToString(),
                                                             Text = sendMethod.SendMethodName
                                                         });

            return new SelectList(sendMethodList, "Value", "Text");
        }
    }
}