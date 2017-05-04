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
        public string ThreadID { get; set; }

        [Display(Name = "Reference ID")]
        public string ReferenceID { get; set; }
        [Display(Name = "Type")]
        public string NotificationType { get; set; }
        [Display(Name = "Level of Impact")]
        public string LevelOfImpact { get; set; }
        [Display(Name = "Heading")]
        public string NotificationHeading { get; set; }
        [Display(Name = "Status")]
        public string Status { get; set; }
        public DateTime SentDateTime { get; set; }
    }

    public class NotificationCreateVM
    {
        public string ThreadID { get; set; }

        [Required]
        public string NotificationHeading { get; set; }

        [Required]
        [AllowHtml]
        public string NotificationDescription { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyy-MM-ddThh:mm}", ApplyFormatInEditMode = true)]
        public DateTime? StartDateTime { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyy-MM-ddThh:mm}", ApplyFormatInEditMode = true)]
        public DateTime? EndDateTime { get; set; }

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

        public string[] ServerReferenceIDs { get; set; }

        public string[] ApplicationReferenceIDs { get; set; }

        public IEnumerable<ApplicationOptionVM> ApplicationList { get; set; }

        public SelectList ServerList { get; set; }

        public SelectList NotificationTypeList { get; set; }

        public SelectList LevelOfImpactList { get; set; }

        public SelectList StatusList { get; set; }

        public SelectList SendMethodList { get; set; }

        public SelectList ProirityList { get; set; }

    }

    public class NotificationDetailVM
    {
        [Display(Name = "Thread ID")]
        public string ThreadID { get; set; }
        [Display(Name = "Source")]
        public string Source { get; set; }
        [Display(Name = "Application/Server")]
        public string ApplicationServerName { get; set; }
        [Display(Name = "Type")]
        public string NotificationType { get; set; }
        [Display(Name = "Level of Impact")]
        public string LevelOfImpact { get; set; }
        [Display(Name = "Status")]
        public string Status { get; set; }
        [Display(Name = "Start Time")]
        public DateTime? StartDateTime { get; set; }
        [Display(Name = "End Time")]
        public DateTime? EndDateTime { get; set; }
        public IEnumerable<NotificationSpecificDetailVM> Thread { get; set; }
        public IEnumerable<NotificationServerVM> Servers { get; set; }
        public IEnumerable<NotificationApplicationVM> Applications { get; set; }
    }
    public class NotificationSpecificDetailVM
    {
        public string ReferenceID { get; set; }
        public string NotificationHeading { get; set; }
        public string NotificationDescription { get; set; }
        public DateTime SentDateTime { get; set; }
    }
    public class NotificationServerVM
    {
        [Display(Name ="Server")]
        public string ServerName { get; set; }
        [Display(Name = "Type")]
        public string ServerType { get; set; }
        [Display(Name = "Status")]
        public string ServerStatus { get; set; }
        public string ReferenceID { get; set; }
    }
    public class NotificationApplicationVM
    {
        [Display(Name = "Application")]
        public string ApplicationName { get; set; }
        [Display(Name = "URL")]
        public string ApplicationURL { get; set; }
        [Display(Name = "Status")]
        public string ApplicationStatus { get; set; }
        public string ReferenceID { get; set; }
    }

    public class ApplicationOptionVM
    {
        public string ReferenceID { get; set; }
        public string ApplicationName { get; set; }
        public string ServerReferenceIDs { get; set; }
    }
}