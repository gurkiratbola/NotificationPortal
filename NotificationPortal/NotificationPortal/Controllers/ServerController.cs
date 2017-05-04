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
    [Authorize(Roles = "Admin, Staff")]
    public class ServerController : Controller
    {
        private readonly ServerRepo _sRepo = new ServerRepo();

        [HttpGet]
        public ActionResult Index()
        {
            IEnumerable<ServerVM> serverList = _sRepo.GetServerList();
            return View(serverList);
        }

        [HttpGet]
        public ActionResult Create()
        {
            // To be modified: global method for status in development
            var model = new ServerVM
            {
                StatusList = _sRepo.GetStatusList(),
                LocationList = _sRepo.GetLocationList()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ServerVM model)
        {
            string msg = "";
            if (ModelState.IsValid)
            {
                bool success = _sRepo.AddServer(model, out msg);
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
            model.StatusList = _sRepo.GetStatusList();
            model.LocationList = _sRepo.GetLocationList();
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            ServerVM server = _sRepo.GetServer(id);
            // To be modified: global method for status in development
            ViewBag.StatusNames = _sRepo.GetStatusList();
            ViewBag.LocationNames = _sRepo.GetStatusList();
            return View(server);
        }

        [HttpPost]
        public ActionResult Edit(ServerVM model)
        {
            string msg = "";
            if (ModelState.IsValid)
            {
                bool success = _sRepo.EditServer(model, out msg);
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
            ServerVM server = _sRepo.GetServer(model.ReferenceID);
            ViewBag.StatusNames = _sRepo.GetStatusList();
            ViewBag.LocationNames = _sRepo.GetLocationList();
            return View(server);
        }

        [HttpGet]
        public ActionResult Details(string id)
        {
            return View(_sRepo.GetServer(id));
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            return View(_sRepo.GetDeleteServer(id));
        }

        [HttpPost]
        public ActionResult Delete(ServerVM server)
        {
            string msg = "";
            if (ModelState.IsValid)
            {
                bool success = _sRepo.DeleteServer(server.ReferenceID, out msg);
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

            return View(server);
        }
    }
}