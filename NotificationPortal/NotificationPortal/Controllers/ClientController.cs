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
    public class ClientController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        private ClientRepo cRepo = new ClientRepo();

        public string FindUserID()
        {
            string name = User.Identity.Name;
            UserDetail user = context.UserDetail
                    .Where(u => u.User.UserName == name).FirstOrDefault();
            string userId = user.UserID;
            return userId;
        }

        [Authorize]
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.ActionMsg = TempData["ActionResultMsg"];
            IEnumerable<ClientVM> clientList = cRepo.GetClientList();
            return View(clientList);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Add()
        {
            var model = new ClientVM
            {
                StatusList = cRepo.GetStatusList()
            };
            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(ClientVM model)
        {
            //TO DO, cannot add client with the same name
            ViewBag.ActionMsg = TempData["ActionResultMsg"];
            string msg = "";
            if (ModelState.IsValid)
            {
                if (cRepo.AddClient(model, out msg)) {
                    return RedirectToAction("index");
                }
                TempData["ActionResultMsg"] = msg;
            }
            else {
                TempData["ActionResultMsg"] = "Client cannot be added at this time.";
            }

            return View();
        }

        [Authorize]
        [HttpGet]
        public ActionResult Edit(int id) {
            string userID = FindUserID();
            ClientVM client = cRepo.GetClient(id, userID);
            ViewBag.StatusNames = cRepo.GetStatusList();
            return View(client);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(ClientVM model) {
            //TO DO, cannot add client with the same name
            ViewBag.ActionMsg = TempData["ActionResultMsg"];
            bool isClientUpdated;
            string userID = FindUserID();
            string msg = "";
            if (ModelState.IsValid)
            {
                isClientUpdated = cRepo.EditClient(model, out msg);
                if (isClientUpdated)
                {
                    TempData["ActionResultMsg"] = "Client information updated.";
                    return RedirectToAction("Details", new { id = model.ClientID });
                }
                else
                {
                    TempData["ActionResultMsg"] = "Failed to update client information at this time.";
                }
            }
            ClientVM client = cRepo.GetClient(model.ClientID, userID);
            ViewBag.StatusNames = cRepo.GetStatusList();
            return View(client);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Details(int id) {
            string userID = FindUserID();
            return View(cRepo.GetClient(id, userID));
        }

        [Authorize]
        [HttpGet]
        public ActionResult Delete(int id) {
            string userID = FindUserID();
            return View(cRepo.GetClient(id, userID));
        }

        [Authorize]
        [HttpPost]
        public ActionResult Delete(ClientVM client) {
            string msg = "";
            cRepo.DeleteClient(client.ClientID, out msg);
            TempData["ActionResultMsg"] = msg;
            return RedirectToAction("Index");
        }
    }
}