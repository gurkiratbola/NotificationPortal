using NotificationPortal.Repositories;
using NotificationPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NotificationPortal.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        public string GetTimeZoneOffset() {
            string timeOffsetString = "0";
            if (Request.Cookies["timezoneoffset"] != null)
            {
                timeOffsetString = Request.Cookies["timezoneoffset"].Value;
            }
            return timeOffsetString;
        }
        public ActionResult Index()
        {
            NotificationRepo nRepo = new NotificationRepo();
            var n = nRepo.GetAllNotifications();
            return View(n);
        }
        
        [HttpGet]
        public ActionResult Add()
        {
            NotificationRepo nRepo = new NotificationRepo();
            var model = nRepo.createAddModel();
            return View(model);
        }
        [HttpPost]
        public ActionResult Add(NotificationCreateVM model) {
            string result = "";
            NotificationRepo nRepo = new NotificationRepo();
            if (ModelState.IsValid)
            {
                bool success = nRepo.CreateNotification(model, out result);
                if (success)
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                ViewBag.ErrorMsg = "Cannot add Notification, model not valid.";
            }
            model = nRepo.createAddModel(model);
            return View(model);
        }
        
        public ActionResult Update()
        {
            return View();
        }
        
        public ActionResult Details()
        {
            return View();
        }
        
        public ActionResult Delete()
        {
            return View();
        }
    }
}