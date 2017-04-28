using NotificationPortal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationPortal.ViewModels
{
  

        public class ServerVM 
        {
            [Key]
            [Required]
            public int ServerID { get; set; }


            [Required]
            public string ServerName { get; set; }

            [Required]
            public string Description { get; set; }


        // public DataCenterLocation location { get; set; }
        //public Status status { get; set; }
        // public Notification notification { get; set; }
         public List<Status> status { get; set; }
         public List<DataCenterLocation> location { get; set; }

        }
    public class ServerEditVM
    {
        [Required]
        public int ServerID { get; set; }

        [Required]
        public int ServerStatusID { get; set; }

        [Required]
        public string ServerName { get; set; }
    }




}
