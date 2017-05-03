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
    [Authorize(Roles = Key.ROLE_ADMIN + "," + Key.ROLE_STAFF)]
    public class ClientController : Controller
    {
        private readonly ClientRepo _cRepo = new ClientRepo();
        private readonly SelectListRepo _sRepo = new SelectListRepo();

        [HttpGet]
        public ActionResult Index()
        {
            IEnumerable<ClientVM> clientList = _cRepo.GetClientList();
            return View(clientList);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var model = new ClientCreateVM
            {
                StatusList = _sRepo.GetStatusList(Key.ROLE_CLIENT)
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClientCreateVM model)
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
        public ActionResult Edit(string id) {
            ClientVM client = _cRepo.GetClient(id);
            ViewBag.StatusNames = _sRepo.GetStatusList(Key.ROLE_CLIENT);
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
                    return RedirectToAction("Details", new { id = model.ReferenceID });
                }
                else
                {
                    TempData["ErrorMsg"] = msg;
                }
            }
            ClientVM client = _cRepo.GetClient(model.ReferenceID);
            ViewBag.StatusNames = _cRepo.GetStatusList();
            return View(client);
        }

        [HttpGet]
        public ActionResult Details(string id) {
            return View(_cRepo.GetClient(id));
        }

        [HttpGet]
        public ActionResult Delete(string id) {
            return View(_cRepo.GetDeleteClient(id));
        }

        [HttpPost]
        public ActionResult Delete(ClientVM client) {
            string msg = "";
            if (ModelState.IsValid)
            {
                bool success = _cRepo.DeleteClient(client.ReferenceID, out msg);
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