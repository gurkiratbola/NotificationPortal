using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NotificationPortal.ViewModels
{
    public class UserVM
    {
        public string UserID { get; set; }

        [Required]
        public string ReferenceID { get; set; }

        [Required]
        public int? ClientID { get; set; }

        [Required]
        public int StatusID { get; set; }

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

        [Display(Name = "Client")]
        public string ClientName { get; set; }

        [Display(Name = "Status")]
        public string StatusName { get; set; }

        [Display(Name = "Status")]
        public SelectList StatusList { get; set; }

        [Display(Name = "Client")]
        public SelectList ClientList { get; set; }
    }

    public class AddUserVM
    {
        public string UserID { get; set; }

        public string ReferenceID { get; set; }

        [Required]
        public int StatusID { get; set; }

        [Required]
        public int? ClientId { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Business Title")]
        public string BusinessTitle { get; set; }

        [Display(Name = "Status")]
        public IEnumerable<SelectListItem> StatusList { get; set; }

        [Display(Name = "Client")]
        public IEnumerable<SelectListItem> ClientList { get; set; }
    }

    public class UserDeleteVM
    {
        [Required]
        public string ReferenceID { get; set; }

        public int? ClientID { get; set; }

        public int StatusID { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Business Title")]
        public string BusinessTitle { get; set; }

        [Display(Name = "Business Phone")]
        public string BusinessPhone { get; set; }

        [Display(Name = "Mobile Phone")]
        public string MobilePhone { get; set; }

        [Display(Name = "Home Phone")]
        public string HomePhone { get; set; }

        [Display(Name = "Client")]
        public string ClientName { get; set; }

        [Display(Name = "Status")]
        public string StatusName { get; set; }

        [Display(Name = "Status")]
        public SelectList StatusList { get; set; }

        [Display(Name = "Client")]
        public SelectList ClientList { get; set; }
    }
}