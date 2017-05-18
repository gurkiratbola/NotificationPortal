using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace NotificationPortal.ViewModels
{
    public class ServerIndexVM
    {
        public IPagedList<ServerListVM> Servers { get; set; }

        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }
        public int TotalItemCount { get; set; }
        public int ItemStart { get; set; }
        public int ItemEnd { get; set; }

        public string StatusSort { get; set; }
        public string LocationSort { get; set; }
        public string DescriptionSort { get; set; }
        public string ServerTypeSort { get; set; }
        public string ServerNameSort { get; set; }
    }

    public class ServerVM
    {
        public string ReferenceID { get; set; }

        [Required]
        [DisplayName("Server Name")]
        public string ServerName { get; set; }

        [Required]
        [DisplayName("Description")]
        public string Description { get; set; }

        [Required]
        [DisplayName("Location")]
        public int LocationID { get; set; }

        [Required]
        [DisplayName("Server Type")]
        public int ServerTypeID { get; set; }

        [Required]
        [DisplayName("Status")]
        public int StatusID { get; set; }

        [Required]
        [Display(Name = "Applications")]
        public string[] ApplicationReferenceIDs { get; set; }
        public SelectList ApplicationList { get; set; }

        public SelectList ServerTypeList { get; set; }
        public SelectList StatusList { get; set; }
        public SelectList LocationList { get; set; }
    }

    public class ServerDetailVM
    {
        public string ReferenceID { get; set; }

        [Required]
        [DisplayName("Server")]
        public string ServerName { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Location")]
        public string Location { get; set; }

        [Required]
        [DisplayName("Location")]
        public int LocationID { get; set; }

        [DisplayName("Server Type")]
        public string ServerType { get; set; }

        [Required]
        [DisplayName("Server Type")]
        public int ServerTypeID { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }

        [Required]
        [DisplayName("Status")]
        public int StatusID { get; set; }

        [Required]
        [Display(Name = "Applications")]
        public string[] ApplicationReferenceIDs { get; set; }
        public SelectList ApplicationList { get; set; }

        public IEnumerable<ServerThreadVM> Threads { get; set; }
        public IEnumerable<ServerApplicationVM> Applications { get; set; }

        public SelectList ServerTypeList { get; set; }
        public SelectList StatusList { get; set; }
        public SelectList LocationList { get; set; }
    }

    public class ServerThreadVM
    {
        public string ReferenceID { get; set; }
        public string ThreadID { get; set; }

        [DisplayName("Heading")]
        public string ThreadHeading { get; set; }

        [DisplayName("Type")]
        public string ThreadType { get; set; }

        [DisplayName("Impact Level")]
        public string LevelOfImpact { get; set; }

        [DisplayName("Status")]
        public string ThreadStatus { get; set; }

        public DateTime SentDateTime { get; set; }
    }

    public class ServerDeleteVM
    {
        public string ReferenceID { get; set; }

        [Required]
        [DisplayName("Server Name")]
        public string ServerName { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [DisplayName("Location")]
        public string Location { get; set; }

        [Required]
        [DisplayName("Server Type")]
        public string ServerTypeName { get; set; }

        [Required]
        [DisplayName("Status")]
        public string StatusName { get; set; }
    }

    public class ServerListVM
    {
        public string ReferenceID { get; set; }

        [Required]
        [DisplayName("Server Name")]
        public string ServerName { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [DisplayName("Location")]
        public string LocationName { get; set; }

        [Required]
        [DisplayName("Server Type")]
        public string ServerTypeName { get; set; }

        [Required]
        [DisplayName("Status")]
        public string StatusName { get; set; }
    }

    public class ServerApplicationVM
    {
        public string ApplicationReferenceID { get; set; }

        [DisplayName("Application")]
        public string ApplicationName { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }

        [DisplayName("URL")]
        public string URL { get; set; }

        [DisplayName("Client")]
        public string ClientID { get; set; }
    }
}
