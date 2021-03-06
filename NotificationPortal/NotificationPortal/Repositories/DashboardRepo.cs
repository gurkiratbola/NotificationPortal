﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NotificationPortal.Models;
using NotificationPortal.ViewModels;
using System.Security.Principal;
using Microsoft.AspNet.Identity;
using PagedList;
using NotificationPortal.Service;

namespace NotificationPortal.Repositories
{
    public class DashboardRepo
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        public DashboardIndexVM GetDashboard(IPrincipal User, string sortOrder, string currentFilter, string searchString, int? page)
        {
            DashboardIndexVM model = null;
            IEnumerable<DashboardVM> dashboard = null;
            if (User != null)
            {
                if (HttpContext.Current.User.IsInRole(Key.ROLE_ADMIN) || HttpContext.Current.User.IsInRole(Key.ROLE_STAFF))
                {
                    // if user is internal
                    try
                    {
                        IEnumerable<DashboardVM> notifications = _context.Notification
                                                                .Select(s => new DashboardVM()
                                                                {
                                                                    ThreadID = s.IncidentNumber,
                                                                    LevelOfImpact = s.LevelOfImpact.LevelName,
                                                                    ImpactValue = s.LevelOfImpact.LevelValue,
                                                                    ThreadHeading = s.NotificationHeading,
                                                                    NotificationType = s.NotificationType.NotificationTypeName,
                                                                    SentDateTime = s.SentDateTime,
                                                                    Status = s.Status.StatusName,
                                                                    SenderName = s.UserDetail.FirstName + " " + s.UserDetail.LastName
                                                                })
                                                                .OrderBy(x => x.LevelOfImpact);

                        dashboard = notifications
                                    .GroupBy(n => n.ThreadID)
                                    .Select(
                                        t => t.OrderByDescending(i => i.SentDateTime).FirstOrDefault()
                        );

                        dashboard = from n in dashboard where n.Status == Key.STATUS_NOTIFICATION_OPEN select n;

                        int totalNumOfNotifications = dashboard.Count();
                        page = searchString == null ? page : 1;
                        int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
                        searchString = searchString ?? currentFilter;
                        int pageNumber = (page ?? 1);
                        int defaultPageSize = ConstantsRepo.PAGE_SIZE;

                        model = new DashboardIndexVM
                        {
                            CurrentFilter = searchString,
                            CurrentSort = sortOrder,
                            TotalItemCount = totalNumOfNotifications,
                            ItemStart = currentPageIndex * defaultPageSize + 1,
                            ItemEnd = totalNumOfNotifications - (defaultPageSize * currentPageIndex) >= defaultPageSize ? defaultPageSize * (currentPageIndex + 1) : totalNumOfNotifications,
                            IDSort = sortOrder == ConstantsRepo.SORT_NOTIFICATION_BY_ID_ASCE ? ConstantsRepo.SORT_NOTIFICATION_BY_ID_DESC : ConstantsRepo.SORT_NOTIFICATION_BY_ID_ASCE,
                            DateSort = sortOrder == ConstantsRepo.SORT_NOTIFICATION_BY_DATE_DESC ? ConstantsRepo.SORT_NOTIFICATION_BY_DATE_ASCE : ConstantsRepo.SORT_NOTIFICATION_BY_DATE_DESC,
                            SubjectSort = sortOrder == ConstantsRepo.SORT_NOTIFICATION_BY_HEADING_DESC ? ConstantsRepo.SORT_NOTIFICATION_BY_HEADING_ASCE : ConstantsRepo.SORT_NOTIFICATION_BY_HEADING_DESC,
                            LevelOfImpactSort = sortOrder == ConstantsRepo.SORT_LEVEL_OF_IMPACT_DESC ? ConstantsRepo.SORT_LEVEL_OF_IMPACT_ASCE : ConstantsRepo.SORT_LEVEL_OF_IMPACT_DESC,
                            SenderSort = sortOrder == ConstantsRepo.SORT_NOTIFICATION_BY_SENDER_DESC ? ConstantsRepo.SORT_NOTIFICATION_BY_SENDER_ASCE : ConstantsRepo.SORT_NOTIFICATION_BY_SENDER_DESC,
                        };
                    }
                    catch
                    {
                        // something wrong with db, catch it by returning null
                        model = null;
                    }
                }
                else if (HttpContext.Current.User.IsInRole(Key.ROLE_CLIENT))
                {
                    // if it's external admin
                    try
                    {
                        var username = User.Identity.Name;
                        var clientID = _context.UserDetail
                                    .Where(u => u.User.UserName == username)
                                    .FirstOrDefault().ClientID;

                        // get all notifications for all client apps
                        var apps = _context.Client
                                    .Where(n => n.ClientID == clientID)
                                    .SingleOrDefault()
                                    .Applications;
                        dashboard = GetAppNotifications(dashboard, apps);
                        model = new DashboardIndexVM
                        {
                            Notifications = Sort(dashboard, sortOrder, searchString).ToPagedList(1, dashboard.Count()),
                        };
                    }
                    catch
                    {
                        // something wrong with db, catch it by returning null
                        model = null;
                    }
                }
                else
                {
                    // if it's external user
                    try
                    {
                        var userId = User.Identity.GetUserId();
                        var apps = _context.UserDetail
                                    .Where(u => u.UserID == userId)
                                    .SingleOrDefault()
                                    .Applications;
                        dashboard = GetAppNotifications(dashboard, apps);
                        model = new DashboardIndexVM
                        {
                            Notifications = Sort(dashboard, sortOrder, searchString).ToPagedList(1, dashboard.Count()),
                        };
                    }
                    catch
                    {
                        // something wrong with db, catch it by returning null
                        model = null;
                    }
                }

            }
            return model;
        }
        public IEnumerable<DashboardThreadDetailVM> GetThreadDetails(string threadID)
        {

            IEnumerable<DashboardThreadDetailVM> details = _context.Notification
                                                           .Where(b => b.IncidentNumber == threadID)
                                                           .Select(c => new DashboardThreadDetailVM
                                                           {
                                                               SentDateTime = c.SentDateTime,
                                                               NotificationDetail = c.NotificationDescription
                                                           }).OrderByDescending(a => a.SentDateTime).ToList()
                                                           .Take(ConstantsRepo.DASHBOARD_CLENT_SIDE_LIMIT);
            foreach (var item in details)
            {
                item.NotificationDetail = StringHelper.RemoveTags(item.NotificationDetail);
            }
            return details;
        }

        public IEnumerable<DashboardVM> GetAppNotifications(IEnumerable<DashboardVM> dashboard, ICollection<Application> apps)
        {
            IEnumerable<DashboardVM> notifications = apps
                                        .Select(x => new { Application = x, x.Servers })
                                        .SelectMany(x => x.Servers.SelectMany(n => n.Notifications.Where(a => a.Applications.Contains(x.Application) || a.Applications.Count() == 0))
                                        .Select(n => new DashboardVM()
                                        {
                                            ThreadID = n.IncidentNumber,
                                            AppName = x.Application.ApplicationName,
                                            LevelOfImpact = n.LevelOfImpact.LevelName,
                                            ImpactValue = n.LevelOfImpact.LevelValue,
                                            ThreadHeading = n.NotificationHeading,
                                            NotificationType = n.NotificationType.NotificationTypeName,
                                            SentDateTime = n.SentDateTime,
                                            Status = n.Status.StatusName
                                        }))
                                        .OrderBy(x => x.LevelOfImpact).ToList();

            dashboard = notifications
                    .GroupBy(n => n.ThreadID)
                    .Select(t => t.OrderBy(i => i.SentDateTime))
                    .Select(n => new { First = n.FirstOrDefault(), Last = n.LastOrDefault() })
                    .Select(
                        t => new DashboardVM()
                        {
                            ThreadID = t.First.ThreadID,
                            AppName = t.Last.AppName,
                            LevelOfImpact = t.Last.LevelOfImpact,
                            ImpactValue = t.Last.ImpactValue,
                            ThreadHeading = t.First.ThreadHeading,
                            NotificationType = t.Last.NotificationType,
                            SentDateTime = t.Last.SentDateTime,
                            Status = t.Last.Status,
                        })
                        .GroupBy(n => n.ThreadID)
                        .Select(
                            t => t.OrderByDescending(i => i.SentDateTime).FirstOrDefault()
                        );

            dashboard = from n in dashboard where n.Status != Key.STATUS_NOTIFICATION_CLOSED select n;

            dashboard = dashboard.ToList();

            foreach (var item in dashboard)
            {
                item.ThreadDetail = GetThreadDetails(item.ThreadID);
            }
            return dashboard;
        }

        public IEnumerable<DashboardVM> Sort(IEnumerable<DashboardVM> list, string sortOrder, string searchString = null)
        {

            if (!String.IsNullOrEmpty(searchString))
            {
                list = list.Where(c => c.LevelOfImpact.ToUpper().Contains(searchString.ToUpper())
                                    || c.ThreadHeading.Contains(searchString));
            }
            switch (sortOrder)
            {
                case ConstantsRepo.SORT_LEVEL_OF_IMPACT_DESC:
                    list = list.OrderByDescending(c => c.LevelOfImpact);
                    break;

                case ConstantsRepo.SORT_LEVEL_OF_IMPACT_ASCE:
                    list = list.OrderBy(c => c.LevelOfImpact);
                    break;

                case ConstantsRepo.SORT_NOTIFICATION_BY_HEADING_ASCE:
                    list = list.OrderBy(c => c.ThreadHeading);
                    break;

                case ConstantsRepo.SORT_NOTIFICATION_BY_HEADING_DESC:
                    list = list.OrderByDescending(c => c.ThreadHeading);
                    break;

                case ConstantsRepo.SORT_NOTIFICATION_BY_ID_ASCE:
                    list = list.OrderBy(c => c.ThreadID);
                    break;

                case ConstantsRepo.SORT_NOTIFICATION_BY_ID_DESC:
                    list = list.OrderByDescending(c => c.ThreadID);
                    break;

                case ConstantsRepo.SORT_NOTIFICATION_BY_DATE_ASCE:
                    list = list.OrderBy(c => c.SentDateTime);
                    break;

                case ConstantsRepo.SORT_NOTIFICATION_BY_DATE_DESC:
                    list = list.OrderByDescending(c => c.SentDateTime);
                    break;

                case ConstantsRepo.SORT_NOTIFICATION_BY_SENDER_ASCE:
                    list = list.OrderBy(c => c.SenderName);
                    break;

                case ConstantsRepo.SORT_NOTIFICATION_BY_SENDER_DESC:
                    list = list.OrderByDescending(c => c.SenderName);
                    break;

                default:
                    list = list.OrderByDescending(c => c.LevelOfImpact);
                    break;
            }
            return list;
        }
    }
}