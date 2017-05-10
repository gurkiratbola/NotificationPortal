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
    public class StatusVM
    {
        
            [Key]
            public int StatusID { get; set; }

            [Required]
            [DisplayName("Status")]
            public string StatusName { get; set; }

            [Required]
            public int StatusTypeID { get; set; }

           
            [DisplayName("Status Type")]
            public string StatusTypeName { get; set; }

            public SelectList StatusTypeList { get; set; }
        
    }
}
