using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NotificationPortal.Models;
using NotificationPortal.Repositories;
using NotificationPortal.ViewModels;

namespace NotificationPortal.Controllers
{
    public class UserController : Controller
    {
        // GET: UserDetails/Index
        [Authorize]
        public ActionResult Index()
        {
            UserRepo userRepo = new UserRepo();
            IEnumerable<UserVM> users = userRepo.GetAll();

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
            return View();
        }

        // POST: UserDetails/Add
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(UserVM model)
        {
            return View();
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
        public ActionResult Details()
        {
            return View();
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
    }
}