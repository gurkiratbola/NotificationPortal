using NotificationPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NotificationPortal.Controllers
{
    public class AppBaseController : Controller
    {
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (User != null)
            {
                ApplicationDbContext _context = new ApplicationDbContext();
                var username = User.Identity.Name;

                if (!string.IsNullOrEmpty(username))
                {
                    string fullName = "";
                    var user = _context.Users.SingleOrDefault(u => u.UserName == username);
                    if (user != null)
                    {
                        fullName = string.Concat(new string[] { user.UserDetail.FirstName, " ", user.UserDetail.LastName });

                    }
                    else {
                        fullName = "user";
                    }
                    ViewData.Add("FullName", fullName);
                }
            }
            base.OnActionExecuted(filterContext);
        }
        public AppBaseController()
        { }
    }
}