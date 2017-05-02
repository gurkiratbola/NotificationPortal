using NotificationPortal.Models;
using NotificationPortal.Repositories;
using NotificationPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static NotificationPortal.ViewModels.ValidationVM;

namespace NotificationPortal.Controllers
{
    [Authorize(Roles="Admin, Staff")]
    public class ClientController : Controller
    {
        private readonly ClientRepo _cRepo = new ClientRepo();

        [HttpGet]
        public ActionResult Index()
        {
            IEnumerable<ClientVM> clientList = _cRepo.GetClientList();
            return View(clientList);
        }

        [HttpGet]
        public ActionResult Create()
        {
            // To be modified: global method for status in development
            var model = new ClientVM
            {
                StatusList = _cRepo.GetStatusList()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClientVM model)
        {
            string msg = "";
            if (ModelState.IsValid)
            {
                bool success = _cRepo.AddClient(model, out msg);
                if (success)
                {
                    TempData["SuccessMsg"] = msg;
                    return RedirectToAction("index");
                }
                else {
                    TempData["ErrorMsg"] = msg;
                }
            }
            else {
                TempData["ErrorMsg"] = "Client cannot be added at this time.";
            }
            model.StatusList = _cRepo.GetStatusList();
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id) {
            ClientVM client = _cRepo.GetClient(id);
            // To be modified: global method for status in development
            ViewBag.StatusNames = _cRepo.GetStatusList();
            return View(client);
        }

        [HttpPost]
        public ActionResult Edit(ClientVM model) {
            string msg = "";
            if (ModelState.IsValid)
            {
                bool success = _cRepo.EditClient(model, out msg);
                if (success)
                {
                    TempData["SuccessMsg"] = msg;
                    return RedirectToAction("Details", new { id = model.ClientID });
                }
                else
                {
                    TempData["ErrorMsg"] = msg;
                }
            }
            ClientVM client = _cRepo.GetClient(model.ClientID);
            ViewBag.StatusNames = _cRepo.GetStatusList();
            return View(client);
        }

        [HttpGet]
        public ActionResult Details(int id) {
            return View(_cRepo.GetClient(id));
        }

        [HttpGet]
        public ActionResult Delete(int id) {
            return View(_cRepo.GetClient(id));
        }

        [HttpPost]
        public ActionResult Delete(ClientVM client) {
            string msg = "";
            if (ModelState.IsValid)
            {
                bool success = _cRepo.DeleteClient(client.ClientID, out msg);
                if (success)
                {
                    TempData["SuccessMsg"] = msg;
                    return RedirectToAction("index");
                }
                else
                {
                    TempData["ErrorMsg"] = msg;
                }
            } else {
                TempData["ErrorMsg"] = "Client cannot be deleted at this time.";
            }

            return View(client);
        }
    }
}