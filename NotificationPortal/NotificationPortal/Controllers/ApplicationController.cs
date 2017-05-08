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
    [Authorize(Roles = Key.ROLE_ADMIN + "," + Key.ROLE_STAFF)]
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
        [HttpGet]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.ClientNameSort =
              String.IsNullOrEmpty(sortOrder) ? ConstantsRepo.SORT_CLIENT_BY_NAME_DESC : "";
            ViewBag.StatusNameSort =
              sortOrder == ConstantsRepo.SORT_STATUS_BY_NAME_DESC ? ConstantsRepo.SORT_STATUS_BY_NAME_ASCE : ConstantsRepo.SORT_STATUS_BY_NAME_DESC;

            // TO DO: if it's null, redirect to a page
            IEnumerable<ApplicationListVM> appList = _aRepo.GetApplicationList();
            appList = _aRepo.Sort(appList, sortOrder, searchString);

            int pageNumber = (page ?? 1);
            return View(appList.ToPagedList(pageNumber, ConstantsRepo.PAGE_SIZE));
        }

        [HttpGet]
        public ActionResult Create()
        {
            // To be modified: global method for status in development
            var model = new ApplicationVM
            {
                StatusList = _sRepo.GetStatusList(Key.STATUS_TYPE_APPLICATION),
                //StatusList = _aRepo.GetStatusList(),
                ClientList = _sRepo.GetClientList(),
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
            model.StatusList = _sRepo.GetStatusList(Key.STATUS_TYPE_APPLICATION);

            model.ClientList = _sRepo.GetClientList();
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            ApplicationVM application = _aRepo.GetApplication(id);
            // To be modified: global method for status in development
            ViewBag.StatusList = _sRepo.GetStatusList(Key.STATUS_TYPE_APPLICATION);

            ViewBag.ClientList = _sRepo.GetClientList();
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
            //ViewBag.ClientRefID = _sRepo.GetStatusList(Key.STATUS_TYPE_APPLICATION);
            //ViewBag.StatusID = _sRepo.GetStatusList(Key.STATUS_TYPE_APPLICATION);
            // ViewBag.ClientID = _sRepo.GetClientList();
            ViewBag.StatusList = _sRepo.GetStatusList(Key.STATUS_TYPE_APPLICATION);
            ViewBag.ClientList = _sRepo.GetClientList();
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