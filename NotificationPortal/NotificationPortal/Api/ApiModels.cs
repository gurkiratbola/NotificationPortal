using NotificationPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotificationPortal.Api
{
    public class IndexFiltered
    {
        public int ItemStart { get; set; }
        public int ItemEnd { get; set; }
        public int PageCount { get; set; }
        public int PageNumber { get; set; }
        public int TotalItemsCount { get; set; }
    }
    public class IndexBody
    {
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }
        public string SearchString { get; set; }
        public int Page { get; set; }
        public int? ItemsPerPage { get; set; }
    }

    public class ApplicationStatus
    {
        public string ReferenceID { get; set; }
        public string Status { get; set; }
    }

    public class DashboardVM
    {
        public string IncidentNumber { get; set; }
        public string ThreadHeading { get; set; }
        public DateTime SentDateTime { get; set; }
        public string SenderName { get; set; }
        public string LevelOfImpact { get; set; }
        public string Status { get; set; }
    }

    public class DashboardIndexFiltered : IndexFiltered
    {
        public List<DashboardVM> Threads { get; set; }
        public string IDSort { get; set; }
        public string SubjectSort { get; set; }
        public string SenderSort { get; set; }
        public string DateSort { get; set; }
        public string LevelOfImpactSort { get; set; }
    }

    public class NotificationIndexFiltered : IndexFiltered
    {
        public List<NotificationThreadVM> Threads { get; set; }
        public string IncidentNumberSort { get; set; }
        public string LevelOfImpactSort { get; set; }
        public string NotificationHeadingSort { get; set; }
        public string NotificationTypeSort { get; set; }
        public string PrioritySort { get; set; }
        public string StatusSort { get; set; }
    }
    public class NotificationIndexBody: IndexBody
    {
        public int[] NotificationTypeIDs { get; set; }
        public int[] LevelOfImpactIDs { get; set; }
        public int[] StatusIDs { get; set; }
        public int[] PriorityIDs { get; set; }
    }
}