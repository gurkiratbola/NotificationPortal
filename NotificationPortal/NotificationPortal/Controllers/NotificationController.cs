using NotificationPortal.Repositories;
using NotificationPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NotificationPortal.Controllers
{
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
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public ActionResult Add()
        {
            NotificationRepo nRepo = new NotificationRepo();
            var model = new NotificationVM
            {
                ApplicationList = nRepo.GetApplicaitonList(),
                ServerList = nRepo.GetServerList(),
                TypeList = nRepo.GetTypeList(),
                LevelOfImpactList = nRepo.GetImpactLevelList(),
                StatusList = nRepo.GetNotificationSatusList()
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult Add(NotificationVM notification) {
            string result = "";
            if (ModelState.IsValid)
            {
                NotificationRepo nRepo = new NotificationRepo();
                nRepo.CreateNotification(notification, out result);
            }
            else
            {
                ViewBag.ErrorMsg = "Cannot add Notification, model not valid.";
            }
            return View();
        }

        [Authorize]
        public ActionResult Update()
        {
            return View();
        }

        [Authorize]
        public ActionResult Details()
        {
            return View();
        }

        [Authorize]
        public ActionResult Delete()
        {
            return View();
        }
    }
}