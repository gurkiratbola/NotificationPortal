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
    [Authorize(Roles = Key.ROLE_ADMIN + "," + Key.ROLE_STAFF)]
    public class DataCenterController : AppBaseController
    {
        ApplicationDbContext _context = new ApplicationDbContext();

        private readonly DataCenterRepo _dRepo = new DataCenterRepo();
        private readonly SelectListRepo _lRepo = new SelectListRepo();

        [HttpGet]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            DataCenterIndexVM dataCenterList = _dRepo.GetDataCenterList(sortOrder, currentFilter, searchString, page);
            return View(dataCenterList);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DataCenterVM model)
        {
            string msg = "";
            if (ModelState.IsValid)
            {
                bool success = _dRepo.AddDataCenter(model, out msg);
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
                TempData["ErrorMsg"] = "Data Center Location cannot be added at this time.";
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            DataCenterVM dataCenter = _dRepo.GetDataCenter(id);
            if (dataCenter == null)
            {
                // if data center is null, redirect to index
                TempData["ErrorMsg"] = "Cannot edit this data center at the moment";
                return RedirectToAction("Index");
            }
            return View(dataCenter);
        }

        [HttpPost]
        public ActionResult Edit(DataCenterVM model)
        {
            string msg = "";
            if (ModelState.IsValid)
            {
                bool success = _dRepo.EditDataCenter(model, out msg);
                if (success)
                {
                    TempData["SuccessMsg"] = msg;
                    return RedirectToAction("Details", new { id = model.LocationID });
                }
                else
                {
                    TempData["ErrorMsg"] = msg;
                }
            }
            DataCenterVM dataCenter = _dRepo.GetDataCenter(model.LocationID);
            return View(dataCenter);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            return View(_dRepo.GetDataCenter(id));
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            return View(_dRepo.GetDataCenter(id));
        }

        [HttpPost]
        public ActionResult Delete(DataCenterVM dataCenter)
        {
            string msg = "";
            if (ModelState.IsValid)
            {
                bool success = _dRepo.DeleteDataCenter(dataCenter.LocationID, out msg);
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
                TempData["ErrorMsg"] = "Data Center Location cannot be deleted at this time.";
            }

            return View(dataCenter);
        }
    }
}