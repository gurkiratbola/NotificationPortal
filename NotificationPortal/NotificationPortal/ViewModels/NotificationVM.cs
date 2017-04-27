using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NotificationPortal.ViewModels
{
    public class NotificationVM
    {
        [Required]
        public int NotificationID { get; set; }

        [Required]
        public int ThreadID { get; set; }

        [Required]
        public string ReferenceID { get; set; }

        [Required]
        public string NotificaionHeading { get; set; }

        [Required]
        public string NotificaionDescription { get; set; }

        [Required]
        public DateTime SentDateTime { get; set; }

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
        public int ServerID { get; set; }

        //application is nullable, server isn't?
        public int? ApplicationID { get; set; }

        public IEnumerable<SelectListItem> ApplicationList { get; set; }

        public IEnumerable<SelectListItem> TypeList { get; set; }

        public IEnumerable<SelectListItem> LevelOfImpactList { get; set; }

        public IEnumerable<SelectListItem> StatusList { get; set; }  

        public IEnumerable<SelectListItem> ProirityList { get; set; }

    }
}