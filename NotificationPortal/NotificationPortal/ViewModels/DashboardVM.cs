using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NotificationPortal.ViewModels
{
    public class DashboardVM
    {
        public string ThreadID { get; set; }

        public string SourceReferenceID { get; set; }

        [Display(Name = "Name")]
        public string SourceName { get; set; }

        [Display(Name = "Level of Impact")]
        public string LevelOfImpact { get; set; }

        [Display(Name = "Heading")]
        public string ThreadHeading { get; set; }

        [Display(Name = "Type")]
        public string NotificationType { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        public DateTime SentDateTime { get; set; }
        
        // for external dashboard only
        public IEnumerable<DashboardThreadDetailVM> ThreadDetail { get; set; }
    }

    public class DashboardThreadDetailVM
    {
        public string NotificationHeading { get; set; }
        public DateTime SentDateTime { get; set; }
    }
}