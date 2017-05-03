using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using NotificationPortal.Models;
using System.Web.Mvc;

namespace NotificationPortal.ViewModels
{

    //public class ServerVM
    //{
    //    [Key]
    //    [Required]
    //    public int ServerID { get; set; }


    //    [Required]
    //    public string ServerName { get; set; }

    //    [Required]
    //    public string Description { get; set; }

    //    [Required]
    //    public int LocationID { get; set; }

    //    [Required]
    //    public int StatusID { get; set; }
    //    // public DataCenterLocation location { get; set; }
    //    //public Status status { get; set; }
    //    // public Notification notification { get; set; }
    //    //public List<Status> status { get; set; }
    //    //public List<DataCenterLocation> location { get; set; }
    //    public IEnumerable<SelectListItem> TypeList { get; set; }
    //    public IEnumerable<SelectListItem> StatusList { get; set; }
    //    public IEnumerable<SelectListItem> LocationList { get; set; }

    //}
    public class StatusVM
    {
        [Key]
        [Required]
        public int StatusID { get; set; }

        [Required]
        public string StatusName { get; set; }

        [Required]
        public int StatusTypeID { get; set; }

        public IEnumerable<SelectListItem> StatusTypeList { get; set; }
    }

    //public class ApplicationVM
    //{
    //    [Key]
    //    [Required]
    //    public int ApplicationID { get; set; }

    //    [Required]
    //    public string ApplicationName { get; set; }

    //    [Required]
    //    public string Description { get; set; }


    //    [Required]
    //    public string URL { get; set; }


    //    [Required]
    //    public int StatusID { get; set; }


    //    [Required]
    //    public int ClientID { get; set; }

    //    public IEnumerable<SelectListItem> StatusList { get; set; }
    //    public IEnumerable<SelectListItem> ClientList { get; set; }
    //}

}
