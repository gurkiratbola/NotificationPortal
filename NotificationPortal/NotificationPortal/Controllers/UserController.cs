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
using PagedList;

namespace NotificationPortal.Controllers
{
    public class UserController : AppBaseController
    {
        private readonly UserRepo _userRepo = new UserRepo();
        private readonly SelectListRepo _selectRepo = new SelectListRepo();

        // GET: UserDetails/Index
        [Authorize(Roles = Key.ROLE_ADMIN + ", " + Key.ROLE_STAFF + ", " + Key.ROLE_CLIENT)]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            UserIndexVM model = _userRepo.GetAllUsers(sortOrder, currentFilter, searchString, page);

            return View(model);
        }

        // GET: UserDetails/Add
        [Authorize(Roles = Key.ROLE_ADMIN + ", " + Key.ROLE_STAFF + ", " + Key.ROLE_CLIENT)]
        [HttpGet]
        public ActionResult Add()
        {
            var model = new AddUserVM()
            {
                StatusList = _selectRepo.GetStatusList(Key.STATUS_TYPE_USER),
                ClientList = _selectRepo.GetClientList(),
                RolesList = _selectRepo.GetRolesList(),
                ApplicationList = _userRepo.GetApplicationList()
            };

            return View(model);
        }

        // POST: UserDetails/Add
        [Authorize(Roles = Key.ROLE_ADMIN + ", " + Key.ROLE_STAFF + ", " + Key.ROLE_CLIENT)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(AddUserVM model)
        {
            if (ModelState.IsValid)
            {
                string msg = "";

                if (_userRepo.AddUser(model, Url, Request, out msg))
                {
                    TempData["SuccessMsg"] = msg;
                  
                    return RedirectToAction("Index");
                }

                TempData["ErrorMsg"] = msg;
            }

            //TempData["ErrorMsg"] = "Cannot add user at this time, please try again!";

            model.StatusList = _selectRepo.GetStatusList(Key.STATUS_TYPE_USER);
            model.ClientList = _selectRepo.GetClientList();
            model.RolesList = _selectRepo.GetRolesList();
            model.ApplicationList = _userRepo.GetApplicationList();

            return View(model);
        }

        // GET: UserDetails/Edit
        [Authorize(Roles = Key.ROLE_ADMIN + ", " + Key.ROLE_STAFF + ", " + Key.ROLE_CLIENT)]
        [HttpGet]
        public ActionResult Edit(string id)
        {
            return View(_userRepo.GetUserDetails(id));
        }

        // POST: UserDetails/Edit
        [Authorize(Roles = Key.ROLE_ADMIN + ", " + Key.ROLE_STAFF + ", " + Key.ROLE_CLIENT)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserVM model)
        {
            if (ModelState.IsValid)
            {
                string msg = "";

                if (_userRepo.EditUser(model, out msg))
                {
                    TempData["SuccessMsg"] = msg;

                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMsg"] = msg;
                }
            }

            model.StatusList = _selectRepo.GetStatusList(Key.STATUS_TYPE_USER);
            model.ClientList = _selectRepo.GetClientList();
            model.RoleList = _selectRepo.GetRolesList();
            model.ApplicationList = _userRepo.GetApplicationList();

            return View(model);
        }

        // GET: UserDetails/Details
        [Authorize(Roles = Key.ROLE_ADMIN + ", " + Key.ROLE_STAFF + ", " + Key.ROLE_CLIENT)]
        [HttpGet]
        public ActionResult Details(string id)
        {
            return View(_userRepo.GetUserDetails(id));
        }

        // GET: UserDetails/Delete
        [Authorize(Roles = Key.ROLE_ADMIN + ", " + Key.ROLE_STAFF + ", " + Key.ROLE_CLIENT)]
        [HttpGet]
        public ActionResult Delete(string id)
        {
            return View(_userRepo.GetDeleteUser(id));
        }

        // POST: UserDetails/Delete
        [Authorize(Roles = Key.ROLE_ADMIN + ", " + Key.ROLE_STAFF + ", " + Key.ROLE_CLIENT)]
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

            //TempData["ErrorMsg"] = "User cannot be deleted at this time.";

            return View(_userRepo.GetDeleteUser(model.ReferenceID));
        }
    }
}
