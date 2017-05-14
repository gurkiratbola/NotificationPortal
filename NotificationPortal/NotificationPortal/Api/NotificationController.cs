using Microsoft.AspNet.Identity;
using NotificationPortal.Models;
using NotificationPortal.Repositories;
using NotificationPortal.ViewModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace NotificationPortal.Api
{
    public class NotificationController : ApiController
    {
        private readonly NotificationRepo _nRepo = new NotificationRepo();
        private readonly SelectListRepo _slRepo = new SelectListRepo();
        // GET: api/Notification
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Notification/5
        public List<NotificationThreadVM> Get([FromUri] int[] currentFilter, string searchString, string sortOrder, int page)
        {
            IEnumerable<NotificationThreadVM> allThreads = _nRepo.GetAllNotifications();

            List<NotificationThreadVM> model = _nRepo.Sort(allThreads, sortOrder, searchString).ToPagedList(page, ConstantsRepo.PAGE_SIZE).ToList();
            return model;
        }

        // POST: api/Notification
        public NotificationIndexFiltered Post([FromBody] NotificationIndexBody model)
        {
            int value = model.NotificationTypeIDs.Length + model.LevelOfImpactIDs.Length + model.StatusIDs.Length + model.PriorityIDs.Length;
            IEnumerable<NotificationThreadVM> allThreads = value==0 ? _nRepo.GetAllNotifications():GetFilteredNotifications(model);
            IPagedList<NotificationThreadVM> threads = _nRepo.Sort(allThreads, model.CurrentSort, model.SearchString).ToPagedList(model.Page, model.ItemsPerPage ?? ConstantsRepo.PAGE_SIZE);
            NotificationIndexFiltered result = new NotificationIndexFiltered()
            {
                ItemStart = (threads.PageNumber-1) * threads.PageSize + 1,
                ItemEnd = threads.PageNumber * threads.PageSize < threads.TotalItemCount ? threads.PageNumber * threads.PageSize : threads.TotalItemCount,
                PageCount = threads.PageCount,
                PageNumber = threads.PageNumber,
                TotalItemsCount = threads.TotalItemCount,
                Threads = threads.ToList(),
                IncidentNumberSort = model.CurrentSort == ConstantsRepo.SORT_INCIDENT_NUMBER_ASCE ? ConstantsRepo.SORT_INCIDENT_NUMBER_DESC: ConstantsRepo.SORT_INCIDENT_NUMBER_ASCE,
                LevelOfImpactSort = model.CurrentSort == ConstantsRepo.SORT_LEVEL_OF_IMPACT_DESC ? ConstantsRepo.SORT_LEVEL_OF_IMPACT_ASCE : ConstantsRepo.SORT_LEVEL_OF_IMPACT_DESC,
                NotificationHeadingSort = model.CurrentSort == ConstantsRepo.SORT_NOTIFICATION_BY_HEADING_ASCE ? ConstantsRepo.SORT_NOTIFICATION_BY_HEADING_DESC : ConstantsRepo.SORT_NOTIFICATION_BY_HEADING_ASCE,
                NotificationTypeSort = model.CurrentSort == ConstantsRepo.SORT_NOTIFICATION_BY_TYPE_ASCE ? ConstantsRepo.SORT_NOTIFICATION_BY_TYPE_DESC: ConstantsRepo.SORT_NOTIFICATION_BY_TYPE_ASCE,
                PrioritySort = model.CurrentSort == ConstantsRepo.SORT_NOTIFICATION_BY_PRIORITY_DESC ? ConstantsRepo.SORT_NOTIFICATION_BY_PRIORITY_ASCE : ConstantsRepo.SORT_NOTIFICATION_BY_PRIORITY_DESC,
                StatusSort = model.CurrentSort == ConstantsRepo.SORT_STATUS_BY_NAME_DESC ? ConstantsRepo.SORT_STATUS_BY_NAME_ASCE : ConstantsRepo.SORT_STATUS_BY_NAME_DESC
            };
            return result;
        }
        
        public class NotificationIndexFiltered
        {
            public int ItemStart { get; set; }
            public int ItemEnd { get; set; }
            public int PageCount { get; set; }
            public int PageNumber { get; set; }
            public int TotalItemsCount { get; set; }
            public List<NotificationThreadVM> Threads { get; set; }
            public string IncidentNumberSort { get; set; }
            public string LevelOfImpactSort { get; set; }
            public string NotificationHeadingSort { get; set; }
            public string NotificationTypeSort { get; set; }
            public string PrioritySort { get; set; }
            public string StatusSort { get; set; }
        }
        public class NotificationIndexBody
        {
            public int[] NotificationTypeIDs { get; set; }
            public int[] LevelOfImpactIDs { get; set; }
            public int[] StatusIDs { get; set; }
            public int[] PriorityIDs { get; set; }

            public string CurrentFilter { get; set; }
            public string CurrentSort { get; set; }
            public string SearchString { get; set; }
            public int Page { get; set; }
            public int? ItemsPerPage { get; set; }
        }

        public IEnumerable<NotificationThreadVM> GetFilteredNotifications(NotificationIndexBody model)
        {
            ApplicationDbContext _context = new ApplicationDbContext();
            try
            {
                model.NotificationTypeIDs = model.NotificationTypeIDs.Length == 0 ? _slRepo.GetTypeList().Select(o => int.Parse(o.Value)).ToArray() : model.NotificationTypeIDs;
                model.LevelOfImpactIDs = model.LevelOfImpactIDs.Length == 0 ? _slRepo.GetImpactLevelList().Select(o => int.Parse(o.Value)).ToArray() : model.LevelOfImpactIDs;
                model.PriorityIDs = model.PriorityIDs.Length == 0 ? _slRepo.GetPriorityList().Select(o => int.Parse(o.Value)).ToArray() : model.PriorityIDs;
                model.StatusIDs = model.StatusIDs.Length == 0 ? _slRepo.GetStatusList(Key.STATUS_TYPE_NOTIFICATION).Select(o => int.Parse(o.Value)).ToArray() : model.StatusIDs;
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
                            LevelOfImpactValue = t.Last.LevelOfImpact.LevelValue,
                            Priority = t.Last.Priority.PriorityName,
                            PriorityValue = t.Last.Priority.PriorityValue,
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

    }
}
