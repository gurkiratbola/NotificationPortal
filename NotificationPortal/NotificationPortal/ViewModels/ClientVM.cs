using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NotificationPortal.ViewModels
{
    public class ClientCreateVM
    {
        [Required]
        [DisplayName("Client")]
        public string ClientName { get; set; }

        [Required]
        public int StatusID { get; set; }

        [DisplayName("Status")]
        public string StatusName { get; set; }

        public SelectList StatusList { get; set; }
    }

    public class ClientVM
    {
        [Required]
        public string ReferenceID { get; set; }

        [Required]
        [DisplayName("Client")]
        public string ClientName { get; set; }

        [Required]
        public int StatusID { get; set; }

        [DisplayName("Status")]
        public string StatusName { get; set; }

        public SelectList StatusList { get; set; }
    }

}