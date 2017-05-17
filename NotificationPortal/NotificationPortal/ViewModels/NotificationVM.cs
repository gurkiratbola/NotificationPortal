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
        // for table head
        [Display(Name = "Incident #")]
        public string IncidentNumber { get; set; }
        [Display(Name = "Type")]
        public string NotificationType { get; set; }
        [Display(Name = "Level of Impact")]
        public string LevelOfImpact { get; set; }
        [Display(Name = "Heading")]
        public string NotificationHeading { get; set; }
        [Display(Name = "Status")]
        public string Status { get; set; }
        [Display(Name = "Priority")]
        public string Priority { get; set; }

        // for filter
        public int[] NotificationTypeIDs { get; set; }
        public int[] LevelOfImpactIDs { get; set; }
        public int[] StatusIDs { get; set; }
        public int[] PriorityIDs { get; set; }

        [Display(Name = "Type")]
        public SelectList NotificationTypeList { get; set; }

        [Display(Name = "Level of Impact")]
        public SelectList LevelOfImpactList { get; set; }

        [Display(Name = "Status")]
        public SelectList StatusList { get; set; }

        [Display(Name = "Priority")]
        public SelectList PriorityList { get; set; }

        // pagination, filter, sort
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }
        public string SearchString { get; set; }
        public int Page { get; set; }
        public string IncidentNumberSort { get; set; }
        public string LevelOfImpactSort { get; set; }
        public string NotificationHeadingSort { get; set; }
        public string NotificationTypeSort { get; set; }
        public string PrioritySort { get; set; }
        public string StatusSort { get; set; }
    }

    public class NotificationThreadVM
    {
        public string IncidentNumber { get; set; }
        
        public string ReferenceID { get; set; }
        public string NotificationType { get; set; }
        public string LevelOfImpact { get; set; }
        public int LevelOfImpactValue { get; set; }
        public string NotificationHeading { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public int PriorityValue { get; set; }
        public DateTime SentDateTime { get; set; }
    }

    public class NotificationCreateVM
    {
        public string IncidentNumber { get; set; }

        [Required]
        [Display(Name = "Heading")]
        public string NotificationHeading { get; set; }

        [Required]
        [AllowHtml]
        [Display(Name = "Description")]
        public string NotificationDescription { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:yyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Start Date")]
        public DateTime? StartDateTime { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:yyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "End Date")]
        public DateTime? EndDateTime { get; set; }

        // ids and select list for dropdown options
        [Display(Name = "Applications")]
        public string[] ApplicationReferenceIDs { get; set; }
        public SelectList ApplicationList { get; set; }

        [Display(Name = "Select Server")]
        public string[] ServerReferenceIDs { get; set; }
        public SelectList ServerList { get; set; }

        [Required]
        public int NotificationTypeID { get; set; }
        [Display(Name = "Type")]
        public SelectList NotificationTypeList { get; set; }

        [Required]
        public int LevelOfImpactID { get; set; }
        [Display(Name = "Level of Impact")]
        public SelectList LevelOfImpactList { get; set; }

        [Required]
        public int StatusID { get; set; }
        [Display(Name = "Status")]
        public SelectList StatusList { get; set; }

        [Required]
        public int PriorityID { get; set; }
        [Display(Name = "Priority")]
        public SelectList PriorityList { get; set; }
    }

    // extends from NotificationCreateVM,
    // but needs NotificationReferenceID to
    // edit specific notification in thread
    public class NotificationEditVM : NotificationCreateVM
    {
        [Required]
        public string NotificationReferenceID { get; set; }
    }

    public class ThreadDetailVM
    {
        [Required]
        [Display(Name = "Incident Number")]
        public string IncidentNumber { get; set; }
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
        [Display(Name = "Sender")]
        public string SenderName { get; set; }
        [Display(Name = "Subject")]
        public string Subject { get; set; }

        // enumerables for the tabs
        public IEnumerable<NotificationDetailVM> Thread { get; set; }
        public IEnumerable<NotificationServerVM> Servers { get; set; }
        public IEnumerable<NotificationApplicationVM> Applications { get; set; }
    }

    // for notification tab on notification detail view
    public class NotificationDetailVM
    {
        public string ReferenceID { get; set; }
        public string Status { get; set; }
        [Display(Name = "Heading")]
        public string NotificationHeading { get; set; }
        [Display(Name = "Details")]
        public string NotificationDescription { get; set; }
        [Display(Name = "Send Time")]
        public DateTime SentDateTime { get; set; }
        [Display(Name = "Incident #")]
        public string IncidentNumber { get; set; }
    }

    // for Server tab on notification detail view
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

    // for Application tab on notification detail view
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
}