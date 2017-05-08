using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

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


    public class ApplicationVM
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

        [Required]
        public string ClientRefID { get; set; }
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

}
