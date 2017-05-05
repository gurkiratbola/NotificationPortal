using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NotificationPortal.ViewModels
{
    public class DashboardExternalVM
    {
        [Required]
        public string ThreadID { get; set; }

        public int ApplicationID { get; set; }

        public int NotificationID { get; set; }

        public string ReferenceID { get; set; }

        public IEnumerable<DashboardThreadDetailVM> ThreadDetail { get; set; }

        [Required]
        public int StatusID { get; set; }

        [Required]
        public int LevelOfImpactID { get; set; }

        [Required]
        public int ServerID { get; set; }

        [Display(Name = "Type")]
        public string NotificationTypeName { get; set; }

        [Display(Name =  "Application Name")]
        public string ApplicationName { get; set; }

        [Display(Name = "Status")]
        public string StatusName { get; set; }


        [Display(Name = "Level of Impact")]
        public string LevelOfImpactName { get; set; }

        [Required]
        [Display(Name = "Heading")]
        public string NotificationHeading { get; set; }

    }

    public class DashboardThreadDetailVM
    {
        public string ReferenceID { get; set; }
        public string NotificationHeading { get; set; }
        public DateTime SentDateTime { get; set; }
    }

    public class DashboardInternalVM
    {
        [Required]
        public string ThreadID { get; set; }

        public int ApplicationID { get; set; }

        public int NotificationID { get; set; }

        public string ReferenceID { get; set; }

        //server status 

        [Required]
        public int StatusID { get; set; }

        [Required]
        public int LevelOfImpactID { get; set; }

        [Required]
        public int ServerID { get; set; }

        [Display(Name = "Type")]
        public string NotificationTypeName { get; set; }

        [Display(Name = "Application Name")]
        public string ApplicationName { get; set; }

        [Display(Name = "Status")]
        public string StatusName { get; set; }

        [Display(Name = "Server")]
        public string ServerName { get; set; }

        [Display(Name = "Level of Impact")]
        public string LevelOfImpactName { get; set; }

        [Required]
        [Display(Name = "Heading")]
        public string NotificationHeading { get; set; }

        [Required]
        [AllowHtml]
        public string NotificationDescription { get; set; }

        public DateTime SentDateTime { get; set; }

        [Required]
        [Display(Name = "Start Time")]
        public DateTime? StartDateTime { get; set; }

    }
}