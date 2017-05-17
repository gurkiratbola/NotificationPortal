using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace NotificationPortal.ViewModels
{
    // view model for index page only
    public class DataCenterIndexVM
    {
        public IPagedList<DataCenterVM> DataCenters { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }
        public string LocationSort { get; set; }
        public int TotalItemCount { get; set; }
        public int ItemStart { get; set; }
        public int ItemEnd { get; set; }
    }
    public class DataCenterVM
    {
        public int LocationID { get; set; }

        [Required]
        public string Location { get; set; }

        public int ServerID { get; set; }

        //public virtual SelectList Servers { get; set; }
    }
}
