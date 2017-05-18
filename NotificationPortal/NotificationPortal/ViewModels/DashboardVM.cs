using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NotificationPortal.ViewModels
{
    // VM for index page only
    public class DashboardIndexVM
    {
        public IPagedList<DashboardVM> Notifications { get; set; }

        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }
        public string LevelOfImpactSort { get; set; }
        public string SubjectSort { get; set; }
        public string SenderSort { get; set; }
        public string IDSort { get; set; }
        public string DateSort { get; set; }
        public int TotalItemCount { get; set; }
        public int ItemStart { get; set; }
        public int ItemEnd { get; set; }
    }
    public class DashboardVM
    {
        [Display(Name = "Incident #")]
        public string ThreadID { get; set; }

        [Display(Name = "Sender")]
        public string SenderName { get; set; }

        [Display(Name = "Level of Impact")]
        public string LevelOfImpact { get; set; }

        public int ImpactValue { get; set; }

        [Display(Name = "Subject")]
        public string ThreadHeading { get; set; }

        [Display(Name = "Creator")]
        public string UserName { get; set; }

        [Display(Name = "Type")]
        public string NotificationType { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "Issue Time")]
        public DateTime SentDateTime { get; set; }

        // for external dashboard only
        public string AppName { get; set; }
        public IEnumerable<DashboardThreadDetailVM> ThreadDetail { get; set; }
    }

    public class DashboardThreadDetailVM
    {
        public string NotificationDetail{ get; set; }
        public DateTime SentDateTime { get; set; }
    }
}