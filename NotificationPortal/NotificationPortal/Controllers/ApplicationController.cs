using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NotificationPortal.Controllers
{
    public class ApplicationController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
    }
}