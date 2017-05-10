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
        [Display(Name = "First Name*")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name*")]
        public string LastName { get; set; }

        [Display(Name = "Business Title")]
        public string BusinessTitle { get; set; }

        [Required]
        [Display(Name = "Business Phone*")]
        public string BusinessPhone { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [Display(Name = "Email* - once changed, system will log out automatically.")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Mobile Phone*")]
        public string MobilePhone { get; set; }

        [Display(Name = "Home Phone")]
        public string HomePhone { get; set; }
    }
}