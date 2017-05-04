using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NotificationPortal.ViewModels
{
    public class ProfileVM
    {
        [Required]
        public string ReferenceID { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Business Title")]
        public string BusinessTitle { get; set; }

        [Required]
        [Display(Name = "Business Phone")]
        public string BusinessPhone { get; set; }

        [Required]
        [Display(Name = "Mobile Phone")]
        public string MobilePhone { get; set; }

        [Required]
        [Display(Name = "Home Phone")]
        public string HomePhone { get; set; }

    }
}