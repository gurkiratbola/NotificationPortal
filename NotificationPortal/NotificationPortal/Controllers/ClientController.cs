using NotificationPortal.Models;
using NotificationPortal.Repositories;
using NotificationPortal.ViewModels;
using System;
using PagedList;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NotificationPortal.Controllers
{
    [Authorize(Roles = Key.ROLE_ADMIN + "," + Key.ROLE_STAFF)]
    public class ClientController : AppBaseController
    {
        private readonly ClientRepo _cRepo = new ClientRepo();
        private readonly SelectListRepo _sRepo = new SelectListRepo();

        [HttpGet]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ClientIndexVM model = _cRepo.GetClientList(sortOrder, currentFilter, searchString, page);
            return View(model);
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
            model.StatusList = _sRepo.GetStatusList(Key.ROLE_CLIENT);
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(string id) {
            ClientVM client = _cRepo.GetClient(id);
            if (client == null)
            {
                // if client is null, redirect to a page
                TempData["ErrorMsg"] = "Cannot edit this client at the moment";
                return RedirectToAction("Index");
            }
            else {
                ViewBag.StatusNames = _sRepo.GetStatusList(Key.ROLE_CLIENT);
                return View(client);
            }
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
            if (client == null)
            {
                // if client is null, redirect to a page
                TempData["ErrorMsg"] = "Cannot edit this client at the moment";
                return RedirectToAction("Index");
            }
            else {
                ViewBag.StatusNames = _sRepo.GetStatusList(Key.ROLE_CLIENT);
                return View(client);
            }
        }

        [HttpGet]
        public ActionResult Details(string id) {
            ClientVM client = _cRepo.GetClient(id);
            if (client == null)
            {
                // if client is null, redirect to a page
                TempData["ErrorMsg"] = "Cannot view this client at the moment";
                return RedirectToAction("Index");
            }
            else {
                return View(client);
            }
        }

        [HttpGet]
        public ActionResult Delete(string id) {
            // TO DO: if it's null, redirect to a page
            ClientVM client = _cRepo.GetClient(id);
            if (client == null)
            {
                // if client is null, redirect to a page
                TempData["ErrorMsg"] = "Cannot delete this client at the moment";
                return RedirectToAction("Index");
            }
            else {
                return View(client);
            }
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