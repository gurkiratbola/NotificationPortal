using NotificationPortal.Models;
using NotificationPortal.Repositories;
using NotificationPortal.ViewModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NotificationPortal.Controllers
{
  
    public class ApplicationController : AppBaseController
    {
        private readonly ApplicationRepo _aRepo = new ApplicationRepo();
        private readonly SelectListRepo _sRepo = new SelectListRepo();

        //[HttpGet]
        //public ActionResult Index()
        //{
        //    IEnumerable<ApplicationListVM> applicationList = _aRepo.GetApplicationList();
        //    return View(applicationList);
        //}
        [Authorize(Roles = Key.ROLE_ADMIN + "," + Key.ROLE_STAFF + "," + Key.ROLE_CLIENT + "," + Key.ROLE_USER)]
        [HttpGet]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {

            ApplicationIndexVM model = _aRepo.GetApplicationList(sortOrder, currentFilter, searchString, page);
            return View(model);
        }
        [Authorize(Roles = Key.ROLE_ADMIN + "," + Key.ROLE_STAFF + "," + Key.ROLE_CLIENT)]
        [HttpGet]
        public ActionResult Create()
        {
            // To be modified: global method for status in development
            var model = new ApplicationVM
            {
                StatusList = _sRepo.GetStatusList(Key.STATUS_TYPE_APPLICATION),
                //StatusList = _aRepo.GetStatusList(),
                ClientList = _sRepo.GetClientList(),
                ServerList = _aRepo.GetServerList(),
            };
            return View(model);
        }
        [Authorize(Roles = Key.ROLE_ADMIN + "," + Key.ROLE_STAFF + "," + Key.ROLE_CLIENT)]
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
            model.StatusList = _sRepo.GetStatusList(Key.STATUS_TYPE_APPLICATION);
            model.ServerList = _aRepo.GetServerList();
            model.ClientList = _sRepo.GetClientList();
            return View(model);
        }

        [Authorize(Roles = Key.ROLE_ADMIN + "," + Key.ROLE_STAFF + "," + Key.ROLE_CLIENT)]
        [HttpGet]
        public ActionResult Edit(string id)
        {
            ApplicationVM application = _aRepo.GetApplication(id);
            // To be modified: global method for status in development
            application.StatusList = _sRepo.GetStatusList(Key.STATUS_TYPE_APPLICATION);
            application.ServerList = _aRepo.GetServerList();
            application.ClientList = _sRepo.GetClientList();
            return View(application);
        }

        [Authorize(Roles = Key.ROLE_ADMIN + "," + Key.ROLE_STAFF + "," + Key.ROLE_CLIENT)]
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
            //ViewBag.ClientRefID = _sRepo.GetStatusList(Key.STATUS_TYPE_APPLICATION);
            //ViewBag.StatusID = _sRepo.GetStatusList(Key.STATUS_TYPE_APPLICATION);
            // ViewBag.ClientID = _sRepo.GetClientList();
            application.StatusList = _sRepo.GetStatusList(Key.STATUS_TYPE_APPLICATION);
            application.ClientList = _sRepo.GetClientList();
            application.ServerList = _aRepo.GetServerList();
            return View(application);
        }

        [Authorize(Roles = Key.ROLE_ADMIN + "," + Key.ROLE_STAFF + "," + Key.ROLE_CLIENT)]
        [HttpGet]
        public ActionResult Details(string id)
        {
            return View(_aRepo.GetDetailApplication(id));
        }
        [Authorize(Roles = Key.ROLE_ADMIN + "," + Key.ROLE_STAFF + "," + Key.ROLE_CLIENT)]
        [HttpGet]
        public ActionResult Delete(string id)
        {
            return View(_aRepo.GetApplication(id));
        }

        [Authorize(Roles = Key.ROLE_ADMIN + "," + Key.ROLE_STAFF + "," + Key.ROLE_CLIENT)]
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