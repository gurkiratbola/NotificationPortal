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
    public class StatusController : AppBaseController
    {
        ApplicationDbContext _context = new ApplicationDbContext();

        private readonly StatusRepo _sRepo = new StatusRepo();
        private readonly SelectListRepo _lRepo = new SelectListRepo();
        // GET: Status
        [HttpGet]
        public ActionResult Index()
        {
            IEnumerable<StatusVM> statusList = _sRepo.GetStatusList();
            return View(statusList);
        }
        [HttpGet]
        public ActionResult Create()
        {
            // To be modified: global method for status in development
            var model = new StatusVM
            {

                StatusTypeList = _lRepo.GetStatusTypeList()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StatusVM model)
        {
            string msg = "";
            if (ModelState.IsValid)
            {
                bool success = _sRepo.AddStatus(model, out msg);
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
                TempData["ErrorMsg"] = "Client cannot be added at this time.";
            }
            model.StatusTypeList = _lRepo.GetStatusTypeList();
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            StatusVM status = _sRepo.GetStatus(id);
            // To be modified: global method for status in development
            ViewBag.StatusTypeList = _lRepo.GetStatusTypeList();
          
            return View(status);
        }

        [HttpPost]
        public ActionResult Edit(StatusVM model)
        {
            string msg = "";
            if (ModelState.IsValid)
            {
                bool success = _sRepo.EditStatus(model, out msg);
                if (success)
                {
                    TempData["SuccessMsg"] = msg;
                    return RedirectToAction("Details", new { id = model.StatusID });
                }
                else
                {
                    TempData["ErrorMsg"] = msg;
                }
            }
            StatusVM status = _sRepo.GetStatus(model.StatusID);
            ViewBag.StatusTypeList = _lRepo.GetTypeList();

            return View(status);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            return View(_sRepo.GetStatus(id));
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            return View(_sRepo.GetStatus(id));
        }

        [HttpPost]
        public ActionResult Delete(StatusVM status)
        {
            string msg = "";
            if (ModelState.IsValid)
            {
                bool success = _sRepo.DeleteStatus(status.StatusID, out msg);
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
                TempData["ErrorMsg"] = "Client cannot be deleted at this time.";
            }

            return View(status);
        }
    }
}