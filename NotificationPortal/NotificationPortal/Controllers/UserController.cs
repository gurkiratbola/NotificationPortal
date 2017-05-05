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
    public class UserController : AppBaseController
    {
        private readonly UserRepo _userRepo = new UserRepo();
        private readonly SelectListRepo _selectRepo = new SelectListRepo();

        // GET: UserDetails/Index
        [Authorize(Roles = Key.ROLE_ADMIN + ", " + Key.ROLE_STAFF)]
        public ActionResult Index(/*string sortOrder, string currentFilter, string searchString, int? page*/)
        {
            //if (searchString != null)
            //{
            //    page = 1;
            //}

            //searchString = currentFilter;

            //ViewBag.CurrentFilter = searchString;
            //ViewBag.CurrentSort = sortOrder;
            //ViewBag.ClientNameSort = string.IsNullOrEmpty(sortOrder) ? ConstantsRepo.SORT_CLIENT_BY_NAME_DESC : "";
            //ViewBag.StatusNameSort = sortOrder == ConstantsRepo.SORT_STATUS_BY_NAME_DESC ? ConstantsRepo.SORT_CLIENT_BY_NAME_ASCE : ConstantsRepo.SORT_STATUS_BY_NAME_DESC;

            IEnumerable<UserVM> users = _userRepo.GetAllUsers();

            return View(users);
        }

        // GET: UserDetails/Add
        [Authorize(Roles = Key.ROLE_ADMIN + ", " + Key.ROLE_STAFF)]
        [HttpGet]
        public ActionResult Add()
        {
            var model = new AddUserVM()
            {
                StatusList = _selectRepo.GetStatusList(Key.STATUS_TYPE_USER),
                ClientList = _selectRepo.GetClientList(),
                RolesList = _selectRepo.GetRolesList(),
            };

            return View(model);
        }

        // POST: UserDetails/Add
        [Authorize(Roles = Key.ROLE_ADMIN + ", " + Key.ROLE_STAFF)]
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

            model.StatusList = _selectRepo.GetStatusList(Key.STATUS_TYPE_USER);
            model.ClientList = _selectRepo.GetClientList();
            model.RolesList = _selectRepo.GetRolesList();

            return View(model);
        }

        // GET: UserDetails/Edit
        [Authorize(Roles = Key.ROLE_ADMIN + ", " + Key.ROLE_STAFF)]
        [HttpGet]
        public ActionResult Edit(string id)
        {
            ViewBag.StatusNames = _selectRepo.GetStatusList(Key.STATUS_TYPE_USER);
            ViewBag.ClientNames = _selectRepo.GetClientList();

            return View(_userRepo.GetUserDetails(id));
        }

        // POST: UserDetails/Edit
        [Authorize(Roles = Key.ROLE_ADMIN + ", " + Key.ROLE_STAFF)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserVM model)
        {
            if (ModelState.IsValid)
            {
                string msg = "";

                if (_userRepo.EditUser(model, out msg))
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
        [Authorize(Roles = Key.ROLE_ADMIN + ", " + Key.ROLE_STAFF)]
        [HttpGet]
        public ActionResult Details(string id)
        {
            ViewBag.StatusNames = _selectRepo.GetStatusList(Key.STATUS_TYPE_USER);
            ViewBag.ClientNames = _selectRepo.GetClientList();

            return View(_userRepo.GetUserDetails(id));
        }

        // GET: UserDetails/Delete
        [Authorize(Roles = Key.ROLE_ADMIN + ", " + Key.ROLE_STAFF)]
        [HttpGet]
        public ActionResult Delete(string id)
        {
            return View(_userRepo.GetDeleteUser(id));
        }

        // POST: UserDetails/Delete
        [Authorize(Roles = Key.ROLE_ADMIN + ", " + Key.ROLE_STAFF)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(UserDeleteVM model)
        {
            if (ModelState.IsValid)
            {
                var msg = "";
                if (_userRepo.DeleteUser(model.ReferenceID, model.ClientReferenceID, out msg))
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
