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
        public string ClientHeadingSort { get; set; }
        public string StatusSort { get; set; }
    }

    public class ServerVM
    {
        [Key]
        public string ReferenceID { get; set; }


        [Required]
        [DisplayName("Server Name*")]
        public string ServerName { get; set; }

        [Required]
        [DisplayName("Server Name*")]
        public string Description { get; set; }

        [Required]
        [DisplayName("Location*")]
        public int LocationID { get; set; }

        [Required]
        [DisplayName("Server Type*")]
        public int ServerTypeID { get; set; }

        [Required]
        [DisplayName("Status*")]
        public int StatusID { get; set; }
        public SelectList ServerTypeList { get; set; }
        public SelectList StatusList { get; set; }
        public SelectList LocationList { get; set; }
    }

    public class ServerDetailVM
    {
        [Key]
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

        public IEnumerable<ServerThreadVM> Threads { get; set; }
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
        [Key]
        public string ReferenceID { get; set; }


        [Required]
        [DisplayName("Server Name")]
        public string ServerName { get; set; }

        [Required]
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


    }

    public class ServerListVM
    {
        [Key]
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
}
