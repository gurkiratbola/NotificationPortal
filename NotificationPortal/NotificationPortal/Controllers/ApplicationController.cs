using NotificationPortal.Models;
using NotificationPortal.Repositories;
using NotificationPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NotificationPortal.Controllers
{
    [Authorize(Roles = Key.ROLE_ADMIN + "," + Key.ROLE_STAFF)]
    public class ApplicationController : AppBaseController
    {
        private readonly ApplicationRepo _aRepo = new ApplicationRepo();
        private readonly SelectListRepo _sRepo = new SelectListRepo();

        [HttpGet]
        public ActionResult Index()
        {
            IEnumerable<ApplicationVM> applicationList = _aRepo.GetApplicationList();
            return View(applicationList);
        }

        [HttpGet]
        public ActionResult Create()
        {
            // To be modified: global method for status in development
            var model = new ApplicationVM
            {
                StatusList = _aRepo.GetStatusList(),
                ClientList = _aRepo.GetClientList(),
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ApplicationVM model)
        {
            string msg = "";
            if (ModelState.IsValid)
            {
                bool success = _aRepo.AddApplication(model, out msg);
                if (success)
                {
                    TempData["SuccessMsg"] = msg;
                    return RedirectToAction("index");
                }
                else
                {
                    TempData["ErrorMsg"] = msg;
                }
            }
            else
            {
                TempData["ErrorMsg"] = "Application cannot be added at this time.";
            }
            model.StatusList = _aRepo.GetStatusList();
            model.ClientList = _aRepo.GetClientList();
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            ApplicationVM application = _aRepo.GetApplication(id);
            // To be modified: global method for status in development
            ViewBag.StatusID = _aRepo.GetStatusList();
            ViewBag.ClientID = _aRepo.GetClientList();
            return View(application);
        }

        [HttpPost]
        public ActionResult Edit(ApplicationVM model)
        {
            string msg = "";
            if (ModelState.IsValid)
            {
                bool success = _aRepo.EditApplication(model, out msg);
                if (success)
                {
                    TempData["SuccessMsg"] = msg;
                    return RedirectToAction("Details", new { id = model.ReferenceID });
                }
                else
                {
                    TempData["ErrorMsg"] = msg;
                }
            }
            ApplicationVM application = _aRepo.GetApplication(model.ReferenceID);
            ViewBag.StatusID = _aRepo.GetStatusList();
            ViewBag.ClientID = _aRepo.GetClientList();
            return View(application);
        }

        [HttpGet]
        public ActionResult Details(string id)
        {
            return View(_aRepo.GetApplication(id));
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            return View(_aRepo.GetApplication(id));
        }

        [HttpPost]
        public ActionResult Delete(ApplicationVM application)
        {
            string msg = "";
            if (ModelState.IsValid)
            {
                bool success = _aRepo.DeleteApplication(application.ReferenceID, out msg);
                if (success)
                {
                    TempData["SuccessMsg"] = msg;
                    return RedirectToAction("index");
                }
                else
                {
                    TempData["ErrorMsg"] = msg;
                }
            }
            else
            {
                TempData["ErrorMsg"] = "Application cannot be deleted at this time.";
            }

            return View(application);
        }


    }
}