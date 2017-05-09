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

        public SelectList GetRolesList()
        {
            IEnumerable<SelectListItem> rolesList = _context.Roles.Select(roles =>
                                                    new SelectListItem
                                                    {
                                                        Value = roles.Name,
                                                        Text = roles.Name
                                                    });

            if (HttpContext.Current.User.IsInRole(Key.ROLE_ADMIN))
            {
                return new SelectList(rolesList, "Value", "Text");
            }

            rolesList = rolesList.Where(r => r.Value != Key.ROLE_ADMIN);

            if (HttpContext.Current.User.IsInRole(Key.ROLE_STAFF))
            {
                return new SelectList(rolesList, "Value", "Text");
            }

            rolesList = rolesList.Where(r => r.Value != Key.ROLE_STAFF);

            return new SelectList(rolesList, "Value", "Text");
        }

        public SelectList GetClientList()
        {
            List<SelectListItem> clientList = _context.Client.Select(app => new SelectListItem
                                              {
                                                  Value = app.ReferenceID.ToString(),
                                                  Text = app.ClientName
                                              }).ToList();

            clientList.Add(new SelectListItem { Value = "-1", Text = "" });
            //clientList.OrderBy(x => x.Text);

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
                                                      Value = app.ReferenceID.ToString(),
                                                      Text = app.ApplicationName
                                                  });

            return new SelectList(appList, "Value", "Text");
        }

        public SelectList GetServerList()
        {
            IEnumerable<SelectListItem> serverList = _context.Server.Select(sv => new SelectListItem()
                                                     {
                                                         Value = sv.ReferenceID.ToString(),
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
                                                         Text = impact.LevelName
                                                     });

            return new SelectList(impactList, "Value", "Text");
        }

        public SelectList GetPriorityList()
        {
            IEnumerable<SelectListItem> priorityList = _context.Priority.Select(p => new SelectListItem()
            {
                Value = p.PriorityID.ToString(),
                Text = p.PriorityName
            });

            return new SelectList(priorityList, "Value", "Text");
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


        public SelectList GetStatusTypeList()
        {
            IEnumerable<SelectListItem> statusTypeList = _context.StatusType.Select(statusType => new SelectListItem()
            {
                Value = statusType.StatusTypeID.ToString(),
                Text = statusType.StatusTypeName,
            });

            return new SelectList(statusTypeList, "Value", "Text");
        }
    }
}