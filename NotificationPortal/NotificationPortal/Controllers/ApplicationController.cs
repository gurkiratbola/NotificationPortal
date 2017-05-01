using NotificationPortal.Models;
using NotificationPortal.Repositories;
using NotificationPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NotificationPortal.Controllers
{
    public class ApplicationController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();

        private IInterfaceRepo<Application> applicationRepo;
        //[Authorize]
        public ApplicationController()
        {
            this.applicationRepo = new ApplicationRepo(new ApplicationDbContext());
        }

        public ApplicationController(IInterfaceRepo<Application> applicationRepo)
        {
            this.applicationRepo = applicationRepo;
        }


        [Authorize]
        public ActionResult Index()
        {
            var applications = from s in applicationRepo.GetAll()
                          select s;
            return View(applications.ToList());
        }

        ///Get /Application/Detials/4
        public ViewResult Details(int ID)
        {

            Application application = applicationRepo.FindBy(ID);
            return View(application);
        }

        //Get: /Applicatoin/Create
        public ActionResult Create()
        {

            ApplicationRepo applicationRepo = new ApplicationRepo(new ApplicationDbContext());
            var model = new ApplicationVM
            {
                StatusList = applicationRepo.GetStatusList(),
                ClientList = applicationRepo.GetClientList(),
            };
            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ServerName, Description,status,location")] ServerVM serverVM) {
        public ActionResult Create(ApplicationVM applicationVM)
        {
            try
            {
                //test variables
                if (ModelState.IsValid)
                {
                    ApplicationDbContext context = new ApplicationDbContext();
                    ///get the location ID and status ID from here

                    Application application = new Application();
                    //server.LocationID = serverVM.location;
                    application.ApplicationName = applicationVM.ApplicationName;
                    application.ApplicationID = applicationVM.StatusID;
                    application.Description = applicationVM.Description;
                    application.URL = applicationVM.URL;
                    application.ClientID = applicationVM.ClientID;
                    application.StatusID = applicationVM.StatusID;
                    applicationRepo.Add(application);
                    applicationRepo.Save();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes. Contact Admin");
            }
            return View(applicationVM);
        }

        public ActionResult Edit(int id)
        {

            ApplicationRepo applicationRepo = new ApplicationRepo(new ApplicationDbContext());
            Application application = applicationRepo.FindBy(id);
            var model = new ApplicationVM
            {
                StatusList = applicationRepo.GetStatusList(),
                ClientList = applicationRepo.GetClientList(),
            };
            model.Description = application.Description;
            model.ApplicationName = application.ApplicationName;
            model.URL = application.URL;
            model.ApplicationID = application.ApplicationID;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ApplicationVM applicationVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationDbContext context = new ApplicationDbContext();
                    ///get the location ID and status ID from here

                    Application application = applicationRepo.FindBy(applicationVM.ApplicationID);
                    //server.LocationID = serverVM.location;
                    application.ApplicationID = applicationVM.ApplicationID;
                    application.StatusID = applicationVM.StatusID;
                    application.ClientID = applicationVM.ClientID;
                    application.Description = applicationVM.Description;
                    application.URL = applicationVM.URL;
                    application.ApplicationName = applicationVM.ApplicationName;
                   // application.ServerID = serverVM.ServerID;
                    applicationRepo.Edit(application);
                    applicationRepo.Save();
                    return RedirectToAction("Index");
                }

            }
            catch (DataException)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes. Contact Admin");
            }
            return View(applicationVM);
        }

        // Get /Application/Delete
        public ActionResult Delete(bool? saveChangesError = false, int ID = 0)
        {
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete Failed, Try Again";
            }
            Application application = applicationRepo.FindBy(ID);
            return View(application);

        }
        //POST: /Application/Delete/3
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int ID)
        {
            try
            {
                Application application = applicationRepo.FindBy(ID);
                applicationRepo.Delete(application);
                applicationRepo.Save();
            }
            catch (DataException)
            {
                return RedirectToAction("Delete", new { ID = ID, saveChangesError = true });
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            applicationRepo.Dispose();
            base.Dispose(disposing);
        }


    }
}