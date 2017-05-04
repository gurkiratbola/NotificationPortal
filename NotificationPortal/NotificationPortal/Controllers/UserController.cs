using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NotificationPortal.Models;
using NotificationPortal.Repositories;
using NotificationPortal.ViewModels;
using System.Net.Mail;
using System.Net;

namespace NotificationPortal.Controllers
{
    public class UserController : Controller
    {
        private readonly UserRepo _userRepo = new UserRepo();

        // GET: UserDetails/Index
        [Authorize]
        public ActionResult Index()
        {
            IEnumerable<UserVM> users = _userRepo.GetAllUsers();

            return View(users);
        }

        // POST: UserDetails/Search
        [Authorize]
        [HttpPost]
        public ActionResult Search()
        {
            return View();
        }

        // GET: UserDetails/Add
        [Authorize]
        [HttpGet]
        public ActionResult Add()
        {
            var model = new AddUserVM()
            {
                StatusList = _userRepo.GetStatusList(),
                ClientList = _userRepo.GetClientList(),
            };

            return View(model);
        }

        // POST: UserDetails/Add
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(AddUserVM model)
        {
            if (ModelState.IsValid)
            {
                string msg = "";

                if (_userRepo.AddUser(model, out msg))
                {
                    TempData["SuccessMsg"] = msg;
                  
                    return RedirectToAction("Index");
                }

                TempData["ErrorMsg"] = msg;
            }

            TempData["ErrorMsg"] = "Cannot add user at this time, please try again!";

            model.StatusList = _userRepo.GetStatusList();
            model.ClientList = _userRepo.GetClientList();

            return View(model);
        }

        // GET: UserDetails/Edit
        [Authorize]
        [HttpGet]
        public ActionResult Edit(string id)
        {
            ViewBag.StatusNames = _userRepo.GetStatusList();
            ViewBag.ClientNames = _userRepo.GetClientList();

            return View(_userRepo.GetUserDetails(id));
        }

        // POST: UserDetails/Edit
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserVM model)
        {
            if (ModelState.IsValid)
            {
                string msg = "";

                bool isUserUpdated = _userRepo.EditUser(model, out msg);

                if (isUserUpdated)
                {
                    TempData["SuccessMsg"] = "User information successfully updated!";

                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMsg"] = "Failed to update the user information.";
                }
            }
            
            return View();
        }


        // GET: UserDetails/Details
        [Authorize]
        [HttpGet]
        public ActionResult Details(string id)
        {
            ViewBag.StatusNames = _userRepo.GetStatusList();
            ViewBag.ClientNames = _userRepo.GetClientList();

            return View(_userRepo.GetUserDetails(id));
        }

        // GET: UserDetails/Delete
        [Authorize]
        [HttpGet]
        public ActionResult Delete(string id)
        {
            return View(_userRepo.GetDeleteUser(id));
        }

        // POST: UserDetails/Delete
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(UserDeleteVM model)
        {
            string msg = "";

            if (ModelState.IsValid)
            {
                if (_userRepo.DeleteUser(model.ReferenceID, out msg))
                {
                    TempData["SuccessMsg"] = msg;

                    return RedirectToAction("Index");
                }

                TempData["ErrorMsg"] = msg;
            }

            TempData["ErrorMsg"] = "User cannot be deleted at this time.";

            return View(model);
        }
    }
}