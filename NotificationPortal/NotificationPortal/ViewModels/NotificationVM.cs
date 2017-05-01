using NotificationPortal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NotificationPortal.ViewModels
{

    public class NotificationIndexVM
    {
        [Display(Name = "Source")]
        public string Source { get; set; }
        [Display(Name = "Application/Server")]
        public string ApplicationServerName { get; set; }
        [Display(Name = "Type")]
        public string NotificationType { get; set; }
        [Display(Name = "Level of Impact")]
        public string LevelOfImpact { get; set; }
        [Display(Name = "Heading")]
        public string NotificationHeading { get; set; }
        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "Sent Time")]
        public DateTime SentDateTime { get; set; }
        [Display(Name = "Start Time")]
        public DateTime StartDateTime { get; set; }
        [Display(Name = "End Time")]
        public DateTime EndDateTime { get; set; }

        [Display(Name = "Client")]
        public string Client { get; set; }
    }

    public class NotificationCreateVM
    {
        public int? ThreadID { get; set; }

        [Required]
        public string NotificationHeading { get; set; }

        [Required]
        [AllowHtml]
        public string NotificationDescription { get; set; }

        [Required]
        public DateTime StartDateTime { get; set; }

        [Required]
        public DateTime EndDateTime { get; set; }

        [Required]
        public int LevelOfImpactID { get; set; }

        [Required]
        public int NotificationTypeID { get; set; }

        [Required]
        public int SentMethodID { get; set; }

        [Required]
        public int StatusID { get; set; }

        [Required]
        public int ProirityID { get; set; }

        public string Source { get; set; }

        public int? ServerID { get; set; }

        public int? ApplicationID { get; set; }

        public SelectList ApplicationList { get; set; }

        public SelectList ServerList { get; set; }

        public SelectList NotificationTypeList { get; set; }

        public SelectList LevelOfImpactList { get; set; }

        public SelectList StatusList { get; set; }

        public SelectList SendMethodList { get; set; }

        public SelectList ProirityList { get; set; }

    }
}