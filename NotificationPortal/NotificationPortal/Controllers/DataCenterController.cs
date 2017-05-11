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

        private readonly DataCenterRepo _sRepo = new DataCenterRepo();
        private readonly SelectListRepo _lRepo = new SelectListRepo();

        // GET: DataCenter
        public ActionResult Index()
        {
            IEnumerable<DataCenterVM> dataCenterList = _sRepo.GetDataCenterList();
            return View(dataCenterList);
        }
        [HttpGet]
        public ActionResult Create()
        {
            // To be modified: global method for status in development
            var model = new DataCenterVM
            {

                Servers = _lRepo.GetServerList()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DataCenterVM model)
        {
            string msg = "";
            if (ModelState.IsValid)
            {
                bool success = _sRepo.AddDataCenter(model, out msg);
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
            model.Servers = _lRepo.GetServerList();
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            DataCenterVM datacenter = _sRepo.GetDataCenter(id);
            // To be modified: global method for status in development
            //ViewBag.ServerList = _lRepo.GetDataCenterList();

            return View(datacenter);
        }

        [HttpPost]
        public ActionResult Edit(DataCenterVM model)
        {
            string msg = "";
            if (ModelState.IsValid)
            {
                bool success = _sRepo.EditDataCenter(model, out msg);
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
            DataCenterVM dataCenter = _sRepo.GetDataCenter(model.LocationID);
            // ViewBag.StatusTypeList = _lRepo.GetTypeList();

            return View(dataCenter);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            return View(_sRepo.GetDataCenter(id));
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            return View(_sRepo.GetDataCenter(id));
        }

        [HttpPost]
        public ActionResult Delete(DataCenterVM dataCenter)
        {
            string msg = "";
            if (ModelState.IsValid)
            {
                bool success = _sRepo.DeleteDataCenter(dataCenter.LocationID, out msg);
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