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
    // view model for index page only
    public class StatusIndexVM
    {
        public IPagedList<StatusVM> Statuses { get; set; }

        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }
        public string ClientHeadingSort { get; set; }
        public string StatusNameSort { get; set; }
        public string StatusTypeSort { get; set; }
        public int TotalItemCount { get; set; }
        public int ItemStart { get; set; }
        public int ItemEnd { get; set; }
    }
    public class StatusVM
    {
        public int StatusID { get; set; }

        [Required]
        [DisplayName("Category Name")]
        public string StatusName { get; set; }

        [Required]
        public int StatusTypeID { get; set; }

        [DisplayName("Status Type")]
        public string StatusTypeName { get; set; }

        public SelectList StatusTypeList { get; set; }
    }
}