using NotificationPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static NotificationPortal.ViewModels.ValidationVM;

namespace NotificationPortal.Repositories
{
    public class ClientRepo
    {
        private ApplicationDbContext context = new ApplicationDbContext();

        //public IEnumerable<SelectListItem> GetCategories(string userId)
        //{
        //    var categories = context.ItemCategories
        //                    .Where(a => a.UserID == userId)
        //                    .Select(x => new SelectListItem
        //                    {
        //                        Value = x.ItemCategoryID.ToString(),
        //                        Text = x.ItemCategoryName
        //                    });

        //    return categories;
        //}
    }
}