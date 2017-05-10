using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using PagedList;

namespace NotificationPortal.ViewModels
{

    public class ApplicationListVM
    {
        [Key]
        public string ReferenceID { get; set; }

        [Required]
        [DisplayName("Application Name")]
        public string ApplicationName { get; set; }

        [Required]
        public string Description { get; set; }


        [Required]
        public string URL { get; set; }


        [Required]
        [DisplayName("Status Name")]
        public string StatusName { get; set; }

        [Required]
        [DisplayName("Client Name")]
        public string ClientName { get; set; }

    }

    public class ApplicationIndexVM
    {
        public IPagedList<ApplicationListVM> Applications { get; set; }

        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }
        public string ApplicationSort { get; set; }
        public string ClientSort { get; set; }
        public string StatusSort { get; set; }
        public string DescriptionSort { get; set; }
        public string URLSort { get; set; }
    }


    public class ApplicationVM
    {
        [Key]
        public string ReferenceID { get; set; }

        [Required]
        [DisplayName("Application Name*")]
        public string ApplicationName { get; set; }

        [Required]
        [DisplayName("Description*")]
        public string Description { get; set; }


        [Required]
        [DisplayName("URL*")]
        public string URL { get; set; }


        [Required]
        [DisplayName("Client*")]
        public string ClientRefID { get; set; }

        [Required]
        [DisplayName("Status*")]
        public int StatusID { get; set; }



        
        [DisplayName("Status*")]
        public string StatusName { get; set; }

        
        [DisplayName("Client*")]
        public string ClientName { get; set; }
        //[Required]
        //public string ClientName { get; set; }

        //[Required]
        //public int ClientID { get; set; }

        public SelectList StatusList { get; set; }
        public SelectList ClientList { get; set; }
    }
    public class ApplicationDeleteVM
    {
        [Key]
        public string ReferenceID { get; set; }

        [Required]
        [DisplayName("Application Name")]
        public string ApplicationName { get; set; }

        [Required]
        public string Description { get; set; }


        [Required]
        public string URL { get; set; }


        //[Required]
        //public string StatusName { get; set; }



        [Required]
        public int StatusID { get; set; }


        //[Required]
        //public string ClientName { get; set; }

        [Required]
        public int ClientID { get; set; }
    }

    public class ApplicationDetailVM
    {
        [Key]
        public string ReferenceID { get; set; }

        [Required]
        [DisplayName("Application Name")]
        public string ApplicationName { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string URL { get; set; }


        [Required]
        public int StatusID { get; set; }

      
        public string Status { get; set; }

        public string Client { get; set; }

        [Required]
        public int ClientID { get; set; }

        public IEnumerable<ApplicationServerVM> Servers { get; set; }
        public IEnumerable<ApplicationUsersVM> Users { get; set; }
        public IEnumerable<ApplicationNotificationsVM> Notifications { get; set; }
        //public IEnumerable<ServerThreadVM> Threads { get; set; }
    }

    public class ApplicationNotificationsVM
    {
        public string ReferenceID { get; set; }

        public string IncidentNumber { get; set; }

        public string ThreadID { get; set; }



        [DisplayName("Description")]
        public string Description { get; set; }


        [DisplayName("Status")]
        public string Status { get; set; }

    }

    public class ApplicationServerVM
    {
        public string ReferenceID { get; set; }


        [DisplayName("Server")]
        public string ServerName { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Location")]
        public string Location { get; set; }

        [DisplayName("Server Type")]
        public string ServerType { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }

    }


    public class ApplicationUsersVM
    {
        public string UserID { get; set; }

        [Required]
        public string ReferenceID { get; set; }

        [Required]
        public string ClientReferenceID { get; set; }

        [Required]
        public int StatusID { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
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

        [Display(Name = "Role")]
        public string RoleName { get; set; }
    }
}
