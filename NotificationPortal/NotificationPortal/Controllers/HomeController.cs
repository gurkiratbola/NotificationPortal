using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NotificationPortal.Controllers
{
    public class HomeController : AppBaseController
    {
        public ActionResult Index()
        {
            //if not logged in, then redirect to account/login
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                return RedirectToAction("Index", "Dashboard");
            }
        }

        public ActionResult Documentation()
        {
            return View();
        }
    }
}