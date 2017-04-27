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
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
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