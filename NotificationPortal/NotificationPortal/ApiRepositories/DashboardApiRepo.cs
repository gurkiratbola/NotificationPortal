using NotificationPortal.ApiModels;
using NotificationPortal.Models;
using NotificationPortal.Repositories;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotificationPortal.ApiRepositories
{
    public class DashboardApiRepo
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();
        private readonly DashboardRepo _dRepo = new DashboardRepo();
        public DashboardIndexFiltered GetFilteredAndSortedDasboard(IndexBody model)
        {
            var dashboardThreads = GetDashboard(model);
            IPagedList<DashboardVM> threads = Sort(dashboardThreads, model.CurrentSort).ToPagedList(model.Page, model.ItemsPerPage ?? ConstantsRepo.PAGE_SIZE);
            DashboardIndexFiltered result = new DashboardIndexFiltered()
            {
                ItemStart = (threads.PageNumber - 1) * threads.PageSize + 1,
                ItemEnd = threads.PageNumber * threads.PageSize < threads.TotalItemCount ? threads.PageNumber * threads.PageSize : threads.TotalItemCount,
                PageCount = threads.PageCount,
                PageNumber = threads.PageNumber,
                TotalItemsCount = threads.TotalItemCount,
                Threads = threads.ToList(),
                IDSort = model.CurrentSort == ConstantsRepo.SORT_NOTIFICATION_BY_ID_ASCE ? ConstantsRepo.SORT_NOTIFICATION_BY_ID_DESC : ConstantsRepo.SORT_NOTIFICATION_BY_ID_ASCE,
                SubjectSort = model.CurrentSort == ConstantsRepo.SORT_NOTIFICATION_BY_HEADING_DESC ? ConstantsRepo.SORT_NOTIFICATION_BY_HEADING_ASCE : ConstantsRepo.SORT_NOTIFICATION_BY_HEADING_DESC,
                SenderSort = model.CurrentSort == ConstantsRepo.SORT_NOTIFICATION_BY_SENDER_DESC ? ConstantsRepo.SORT_NOTIFICATION_BY_SENDER_ASCE : ConstantsRepo.SORT_NOTIFICATION_BY_SENDER_DESC,
                DateSort = model.CurrentSort == ConstantsRepo.SORT_NOTIFICATION_BY_DATE_DESC ? ConstantsRepo.SORT_NOTIFICATION_BY_DATE_ASCE : ConstantsRepo.SORT_NOTIFICATION_BY_DATE_DESC,
                LevelOfImpactSort = model.CurrentSort == ConstantsRepo.SORT_LEVEL_OF_IMPACT_DESC ? ConstantsRepo.SORT_LEVEL_OF_IMPACT_ASCE : ConstantsRepo.SORT_LEVEL_OF_IMPACT_DESC,
            };
            return result;
        }

        public IEnumerable<DashboardVM> GetDashboard(IndexBody model)
        {
            IEnumerable<DashboardVM> dashboard = null;
            if (HttpContext.Current.User.IsInRole(Key.ROLE_ADMIN) || HttpContext.Current.User.IsInRole(Key.ROLE_STAFF))
            {
                IEnumerable<DashboardVM> dashboardNotifications;
                if (String.IsNullOrEmpty(model.SearchString))
                {
                    dashboardNotifications = _context.Notification
                    .Select(s => new DashboardVM()
                    {
                        IncidentNumber = s.IncidentNumber,
                        LevelOfImpact = s.LevelOfImpact.LevelName,
                        ImpactValue = s.LevelOfImpact.LevelValue,
                        ThreadHeading = s.NotificationHeading,
                        SentDateTime = s.SentDateTime,
                        Status = s.Status.StatusName,
                        SenderName = s.UserDetail.FirstName + " " + s.UserDetail.LastName
                    })
                    .OrderBy(x => x.LevelOfImpact);
                }
                else{
                    dashboardNotifications = _context.Notification.Where(
                    n => n.IncidentNumber.ToLower().Contains(model.SearchString.ToLower())
                    || n.NotificationHeading.ToLower().Contains(model.SearchString.ToLower())
                    || n.NotificationDescription.ToLower().Contains(model.SearchString.ToLower())
                    // TODO: add more columns for large index search
                    )
                    .Select(s => new DashboardVM()
                    {
                        IncidentNumber = s.IncidentNumber,
                        LevelOfImpact = s.LevelOfImpact.LevelName,
                        ImpactValue = s.LevelOfImpact.LevelValue,
                        ThreadHeading = s.NotificationHeading,
                        SentDateTime = s.SentDateTime,
                        Status = s.Status.StatusName,
                        SenderName = s.UserDetail.FirstName + " " + s.UserDetail.LastName
                    })
                    .OrderBy(x => x.LevelOfImpact);
                }

                dashboard = dashboardNotifications
                                    .GroupBy(n => n.IncidentNumber)
                                    .Select(
                                        t => t.OrderByDescending(i => i.SentDateTime).FirstOrDefault())
                                        .Where(n=>n.Status != Key.STATUS_NOTIFICATION_CLOSED);
            }
            return dashboard;
        }

        public IEnumerable<DashboardVM> Sort(IEnumerable<DashboardVM> list, string sortOrder)
        {
            switch (sortOrder)
            {
                case ConstantsRepo.SORT_LEVEL_OF_IMPACT_DESC:
                    list = list.OrderByDescending(c => c.ImpactValue);
                    break;

                case ConstantsRepo.SORT_LEVEL_OF_IMPACT_ASCE:
                    list = list.OrderBy(c => c.ImpactValue);
                    break;

                case ConstantsRepo.SORT_NOTIFICATION_BY_HEADING_ASCE:
                    list = list.OrderBy(c => c.ThreadHeading);
                    break;

                case ConstantsRepo.SORT_NOTIFICATION_BY_HEADING_DESC:
                    list = list.OrderByDescending(c => c.ThreadHeading);
                    break;

                case ConstantsRepo.SORT_NOTIFICATION_BY_ID_ASCE:
                    list = list.OrderBy(c => c.IncidentNumber);
                    break;

                case ConstantsRepo.SORT_NOTIFICATION_BY_ID_DESC:
                    list = list.OrderByDescending(c => c.IncidentNumber);
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
                    list = list.OrderByDescending(c => c.ImpactValue);
                    break;
            }
            return list;
        }
    }
}