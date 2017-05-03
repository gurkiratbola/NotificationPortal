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
    public class ServerVM
    {
        [Key]
        [Required]
        public string ReferenceID { get; set; }


        [Required]
        [DisplayName("Server Name")]
        public string ServerName { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int LocationID { get; set; }

        [Required]
        public int StatusID { get; set; }
        // public DataCenterLocation location { get; set; }
        //public Status status { get; set; }
        // public Notification notification { get; set; }
        //public List<Status> status { get; set; }
        //public List<DataCenterLocation> location { get; set; }
       // public IEnumerable<SelectListItem> TypeList { get; set; }
        public SelectList StatusList { get; set; }
        public SelectList LocationList { get; set; }
    }
}
