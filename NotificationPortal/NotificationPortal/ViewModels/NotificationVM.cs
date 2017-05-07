using NotificationPortal.Models;
using PagedList;
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
        public IPagedList<NotificationThreadVM> Threads { get; set; }
        
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }
        public string LevelOfImpactSort { get; set; }
        public string NotificationHeadingSort { get; set; }
        public string NotificationTypeSort { get; set; }
        public string StatusSort { get; set; }
    }

    public class NotificationThreadVM
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
        
        [DisplayFormat(DataFormatString = "{0:yyy-MM-ddThh:mm}", ApplyFormatInEditMode = true)]
        public DateTime? StartDateTime { get; set; }
        
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

        public string[] ServerReferenceIDs { get; set; }

        public string[] ApplicationReferenceIDs { get; set; }

        public IEnumerable<ApplicationServerOptionVM> ApplicationList { get; set; }

        public SelectList ServerList { get; set; }

        public SelectList NotificationTypeList { get; set; }

        public SelectList LevelOfImpactList { get; set; }

        public SelectList StatusList { get; set; }

        public SelectList SendMethodList { get; set; }

        public SelectList ProirityList { get; set; }

    }
    
    public class NotificationEditVM : NotificationCreateVM
    {
        [Required]
        public string NotificationReferenceID { get; set; }
    }

    public class ThreadDetailVM
    {
        [Required]
        [Display(Name = "Thread ID")]
        public string ThreadID { get; set; }
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
        public IEnumerable<NotificationDetailVM> Thread { get; set; }
        public IEnumerable<NotificationServerVM> Servers { get; set; }
        public IEnumerable<NotificationApplicationVM> Applications { get; set; }
    }
    public class NotificationDetailVM
    {
        public string ReferenceID { get; set; }
        public string NotificationHeading { get; set; }
        public string NotificationDescription { get; set; }
        public DateTime SentDateTime { get; set; }
        public string ThreadID { get; set; }
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

    public class ApplicationServerOptionVM
    {
        public string ReferenceID { get; set; }
        public string ApplicationName { get; set; }
        public string ServerReferenceIDs { get; set; }
    }
}