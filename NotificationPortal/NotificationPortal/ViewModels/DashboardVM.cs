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
        [Required]
        public int ApplicationID { get; set; }

        public int NotificationID { get; set; }

        public int ThreadID { get; set; }

        public string ReferenceID { get; set; }

        [Required]
        public int StatusID { get; set; }

        [Required]
        public int ClientID { get; set; }

        [Required]
        public int LevelOfImpactID { get; set; }

        [Required]
        public int NotificationTypeID { get; set; }

        [Required]
        public int SentMethodID { get; set; }

        [Required]
        public int ProirityID { get; set; }

        [Required]
        public int ServerID { get; set; }

        [Required]
        [Display(Name =  "Application Name")]
        public string ApplicationName { get; set; }

        [Display(Name = "Client")]
        public string ClientName { get; set; }

        [Display(Name = "Status")]
        public string StatusName { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Website")]
        public string URL { get; set; }

        [Required]
        public string NotificationHeading { get; set; }

        [Required]
        [AllowHtml]
        public string NotificationDescription { get; set; }

        public DateTime SentDateTime { get; set; }

        [Required]
        public DateTime StartDateTime { get; set; }

        [Required]
        public DateTime EndDateTime { get; set; }

        public IEnumerable<SelectListItem> ApplicationList { get; set; }

        public IEnumerable<SelectListItem> ServerList { get; set; }

        public IEnumerable<SelectListItem> TypeList { get; set; }

        public IEnumerable<SelectListItem> LevelOfImpactList { get; set; }

        public IEnumerable<SelectListItem> StatusList { get; set; }

        public IEnumerable<SelectListItem> ProirityList { get; set; }
    }
}