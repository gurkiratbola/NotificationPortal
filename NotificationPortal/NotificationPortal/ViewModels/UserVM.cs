using PagedList;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace NotificationPortal.ViewModels
{
    public class UserIndexVM
    {
        public IPagedList<UserVM> Users { get; set; }

        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }
        public int TotalItemCount { get; set; }
        public int ItemStart { get; set; }
        public int ItemEnd { get; set; }

        public string RoleNameSort { get; set; }     
        public string EmailSort { get; set; }
        public string FirstNameSort { get; set; }
        public string LastNameSort { get; set; }
        public string ClientHeadingSort { get; set; }
        public string StatusSort { get; set; }
    }

    public class SetPasswordVM
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
    
    public class UserVM
    {
        public string UserID { get; set; }

        [Required]
        public string ReferenceID { get; set; }

        [Display(Name = "Client")]
        public string ClientReferenceID { get; set; }

        [Required]
        [Display(Name = "Status")]
        public int StatusID { get; set; }

        [Required]
        [EmailAddress]
        public string Email  { get; set; }

        [Display(Name = "Applications")]
        public string[] ApplicationReferenceIDs { get; set; }

        [Display(Name = "Applications")]
        public string ApplicationName { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Business Title")]
        public string BusinessTitle { get; set; }

        [Display(Name = "Business Phone")]
        [Phone]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        public string BusinessPhone { get; set; }

        [Display(Name = "Mobile Phone")]
        [Phone]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        public string MobilePhone { get; set; }

        [Display(Name = "Home Phone")]
        [Phone]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        public string HomePhone { get; set; }

        [Display(Name = "Role")]
        public string RoleName { get; set; }

        [Display(Name = "Client")]
        public string ClientName { get; set; }

        [Display(Name = "Status")]
        public string StatusName { get; set; }

        [Display(Name = "Role")]
        public SelectList RoleList { get; set; }

        [Display(Name = "Status")]
        public SelectList StatusList { get; set; }

        [Display(Name = "Client")]
        public SelectList ClientList { get; set; }

        [Display(Name = "Applications")]
        public SelectList ApplicationList { get; set; }

        [Display(Name = "Applications")]
        public IEnumerable<ApplicationVM> Applications { get; set; }
    }

    public class AddUserVM
    {
        public string UserID { get; set; }

        public string ReferenceID { get; set; }

        [Required]
        [Display(Name = "Status")]
        public int StatusID { get; set; }

        [Display(Name = "Client")]
        public string ClientReferenceID { get; set; }

        [Required]
        [Display(Name = "Role")]
        public string RoleName { get; set; }

        [Display(Name = "Applications")]
        public string[] ApplicationReferenceIDs { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Business Title")]
        public string BusinessTitle { get; set; }

        [Display(Name = "Status")]
        public SelectList StatusList { get; set; }

        [Display(Name = "Client")]
        public SelectList ClientList { get; set; }

        [Display(Name = "Role")]
        public SelectList RolesList { get; set; }

        [Display(Name = "Applications")]
        public SelectList ApplicationList { get; set; }
    }

    public class UserDeleteVM
    {
        [Required]
        public string ReferenceID { get; set; }

        public string ClientReferenceID { get; set; }

        public int StatusID { get; set; }

        public string Email { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Applications")]
        public string[] ApplicationReferenceIDs { get; set; }

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

        [Display(Name = "Role")]
        public string RoleName { get; set; }

        [Display(Name = "Role")]
        public SelectList RoleList { get; set; }

        [Display(Name = "Status")]
        public SelectList StatusList { get; set; }

        [Display(Name = "Client")]
        public SelectList ClientList { get; set; }

        [Display(Name = "Applications")]
        public IEnumerable<ApplicationVM> Applications { get; set; }

        [Display(Name = "Applications")]
        public IEnumerable<ApplicationClientOptionVM> ApplicationList { get; set; }
    }


    public class ApplicationClientOptionVM
    {
        public string ReferenceID { get; set; }
        public string ApplicationName { get; set; }
        public string ClientReferenceID { get; set; }
    }
}
