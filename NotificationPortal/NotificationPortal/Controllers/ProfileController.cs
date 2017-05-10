using NotificationPortal.Repositories;
using NotificationPortal.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace NotificationPortal.Controllers
{
    public class ProfileController : AppBaseController
    {
        private readonly ProfileRepo _pRepo = new ProfileRepo();
        // GET: Profile
        [HttpGet]
        public ActionResult Index()
        {
            ProfileVM user = _pRepo.GetUserDetail(User);
            return View(user);
        }
        [HttpPost]
        public ActionResult Index(ProfileVM model) {
            string msg = "";
            if (ModelState.IsValid)
            {
                bool success = _pRepo.EditProfile(model, out msg);
                if (success)
                {
                    if (msg == "Username changed") {
                        //return RedirectToAction("LogOff", "Account");
                        HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                        return RedirectToAction("Index", "Home");
                    }
                    TempData["SuccessMsg"] = msg;
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMsg"] = msg;
                }
            }
            return View(model);
        }
    }
}