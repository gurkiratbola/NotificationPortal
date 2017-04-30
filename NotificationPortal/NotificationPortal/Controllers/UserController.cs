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
            IEnumerable<UserVM> users = _userRepo.GetAll();

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
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));

                var user = new ApplicationUser()
                {
                    UserName = model.Email,
                    Email = model.Email
                };

                userManager.Create(user);

                UserDetail details = new UserDetail()
                {
                    UserID = user.Id,
                    BusinessTitle = model.BusinessTitle,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    StatusID = model.StatusID,
                };

                _context.UserDetail.Add(details);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: UserDetails/Edit
        [Authorize]
        [HttpGet]
        public ActionResult Edit()
        {
            return View();
        }

        // POST: UserDetails/Edit
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserVM model)
        {
            return View();
        }


        // GET: UserDetails/Details
        [Authorize]
        [HttpGet]
        public ActionResult Details(string id)
        {
            return View(_userRepo.GetUserDetails(id));
        }

        // GET: UserDetails/Delete
        [Authorize]
        [HttpGet]
        public ActionResult Delete()
        {
            return View();
        }

        // POST: UserDetails/Delete
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(UserVM model)
        {
            return View();
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