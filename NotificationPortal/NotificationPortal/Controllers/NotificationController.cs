using NotificationPortal.Models;
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
    public class NotificationController : AppBaseController
    {
        private readonly NotificationRepo _nRepo = new NotificationRepo();
        //public string GetTimeZoneOffset() {
        //    string timeOffsetString = "0";
        //    if (Request.Cookies["timezoneoffset"] != null)
        //    {
        //        timeOffsetString = Request.Cookies["timezoneoffset"].Value;
        //    }
        //    return timeOffsetString;
        //}

        public ActionResult Index()
        {
            var n = _nRepo.GetAllNotifications();
            return View(n);
        }

        public ActionResult DetailsThread(string id)
        {
            var model = _nRepo.CreateDetailModel(id);
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = Key.ROLE_ADMIN + "," + Key.ROLE_STAFF)]
        public ActionResult CreateThread()
        {
            var model = _nRepo.CreateAddModel();
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = Key.ROLE_ADMIN + "," + Key.ROLE_STAFF)]
        public ActionResult CreateThread(NotificationCreateVM model) {
            string result = "";
            if (ModelState.IsValid)
            {
                bool success = _nRepo.CreateNotification(model, out result);
                if (success)
                {
                    TempData["SuccessMsg"] = result;
                    return RedirectToAction("Index");
                }
            }
            else
            {
                ViewBag.ErrorMsg = "Cannot add Notification, model not valid.";
            }
            model = _nRepo.CreateAddModel(model);
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = Key.ROLE_ADMIN + "," + Key.ROLE_STAFF)]
        public ActionResult Create(string id)
        {
            var model = _nRepo.CreateUpdateModel(id);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = Key.ROLE_ADMIN + "," + Key.ROLE_STAFF)]
        public ActionResult Create(NotificationCreateVM model)
        {
            string result = "";
            if (ModelState.IsValid)
            {
                bool success = _nRepo.CreateNotification(model, out result);
                if (success)
                {
                    TempData["SuccessMsg"] = result;
                    return RedirectToAction("DetailsThread",new { id = model.ThreadID });
                }
            }
            else
            {
                ViewBag.ErrorMsg = "Cannot update Notification, model not valid.";
            }
            model = _nRepo.CreateUpdateModel(model.ThreadID,model);
            return View(model);
        }
        
        [HttpGet]
        [Authorize(Roles = Key.ROLE_ADMIN + "," + Key.ROLE_STAFF)]
        public ActionResult Edit(string id)
        {
            var model = _nRepo.CreateEditModel(id);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = Key.ROLE_ADMIN + "," + Key.ROLE_STAFF)]
        public ActionResult Edit(NotificationEditVM model)
        {
            string result = "";
            if (ModelState.IsValid)
            {
                bool success = _nRepo.EditNotification(model, out result);
                if (success)
                {
                    TempData["SuccessMsg"] = result;
                    return RedirectToAction("DetailsThread", new { id = model.ThreadID});
                }
            }
            else
            {
                TempData["ErrorMsg"] = "Cannot edit Notification, model not valid.";
            }
            TempData["ErrorMsg"] = result;
            model = _nRepo.CreateEditModel(model.NotificationReferenceID, model);
            return View(model);
        }
        
        [HttpGet]
        [Authorize(Roles = Key.ROLE_ADMIN)]
        public ActionResult Delete(string id)
        {
            var model = _nRepo.CreateDeleteModel(id);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = Key.ROLE_ADMIN)]
        public ActionResult Delete(NotificationDetailVM model)
        {
            string result = "";
            if (ModelState.IsValid)
            {
                bool success = _nRepo.DeleteNotification(model.ReferenceID, out result);
                if (success)
                {
                    TempData["SuccessMsg"] = result;
                    // TODO handle when thread no longer exists
                    return RedirectToAction("DetailsThread", new { id = model.ThreadID });
                }
            }
            else
            {
                TempData["ErrorMsg"] = "Cannot delete Notification, try again later.";
            }
            TempData["ErrorMsg"] = result;
            model = _nRepo.CreateDeleteModel(model.ReferenceID);
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = Key.ROLE_ADMIN)]
        public ActionResult DeleteThread(string id)
        {
            var model = _nRepo.CreateDetailModel(id);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = Key.ROLE_ADMIN)]
        public ActionResult DeleteThread(ThreadDetailVM model)
        {
            string result = "";
            if (ModelState.IsValid)
            {
                bool success = _nRepo.DeleteThread(model.ThreadID, out result);
                if (success)
                {
                    TempData["SuccessMsg"] = result;
                    return RedirectToAction("Index");
                }
            }
            else
            {
                ViewBag.ErrorMsg = "Cannot add Notification, model not valid.";
            }
            TempData["ErrorMsg"] = result;
            model = _nRepo.CreateDetailModel(model.ThreadID);
            return View(model);
        }
    }
}