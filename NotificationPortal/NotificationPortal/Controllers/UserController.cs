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

namespace NotificationPortal.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();
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
                    TempData["AddUserSuccess"] = msg;

                    return RedirectToAction("Index");
                }

                TempData["AddUserError"] = msg;

                return RedirectToAction("Index");
            }

            TempData["AddUserError"] = "Cannot add user at this time, please try again!";

            return View(model);
        }

        // GET: UserDetails/Edit
        [Authorize]
        [HttpGet]
        public ActionResult Edit(string id)
        {
            ViewBag.StatusNames = _userRepo.GetStatusList();
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
                    TempData["EditUserSuccess"] = "User information successfully updated!";

                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["EditUserError"] = "Failed to update the user information.";

                    return RedirectToAction("Index");
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
            return View(_userRepo.GetUserDetails(id));
        }

        // GET: UserDetails/Delete
        [Authorize]
        [HttpGet]
        public ActionResult Delete(string id, int? clientId)
        {
            return View(_userRepo.GetUserDetails(id));
        }

        // POST: UserDetails/Delete
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(UserVM model)
        {
            string msg = "";
            _userRepo.DeleteUser(model.UserID, model.ClientID, out msg);
            TempData["ActionResultMsg"] = msg;

            return RedirectToAction("Index");
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}