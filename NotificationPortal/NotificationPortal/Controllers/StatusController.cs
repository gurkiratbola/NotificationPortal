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
    public class StatusController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();

        private IInterfaceRepo<Status> statusRepo;

        public StatusController()
        {
            this.statusRepo = new StatusRepo(new ApplicationDbContext());
        }

        public StatusController(IInterfaceRepo<Status> statusRepo)
        {
            this.statusRepo = statusRepo;
        }

        // GET: Status
        public ActionResult Index()
        {
            var status = from s in statusRepo.GetAll()
                          select s;
            return View(status.ToList());
        }

        ///Get /Status/Detials/4
        public ViewResult Details(int ID)
        {

            Status status = statusRepo.FindBy(ID);
            return View(status);
        }
        //Get: /Status/Create
        public ActionResult Create()
        {

            StatusRepo statusRepo = new StatusRepo(new ApplicationDbContext());
            var model = new StatusVM
            {
                StatusTypeList = statusRepo.GetStatusTypeList(),
             
            };
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ServerName, Description,status,location")] ServerVM serverVM) {
        public ActionResult Create(StatusVM statusVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationDbContext context = new ApplicationDbContext();
                    ///get the location ID and status ID from here

                    Status status = new Status();
      
                    status.StatusID = statusVM.StatusID;
                    status.StatusName = statusVM.StatusName.ToString();
                    status.StatusTypeID = statusVM.StatusTypeID;
                    statusRepo.Add(status);
                    statusRepo.Save();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes. Contact Admin");
            }
            return View(statusVM);
        }

        public ActionResult Edit(int id)
        {
            StatusRepo statusRepo = new StatusRepo(new ApplicationDbContext());
            Status status = statusRepo.FindBy(id);
            var model = new StatusVM
            {
                StatusTypeList = statusRepo.GetStatusTypeList(),
             
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(StatusVM statusVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                ApplicationDbContext context = new ApplicationDbContext();
                ///get the location ID and status ID from here
                Status status = statusRepo.FindBy(statusVM.StatusID);
                //server.LocationID = serverVM.location;
                status.StatusID = statusVM.StatusID;
                status.StatusName = statusVM.StatusName;
                //status.StatusTypeID = StatusTypeID();
                statusRepo.Edit(status);
                statusRepo.Save();
                return RedirectToAction("Index");

           
                 }
            }
            catch (DataException)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes. Contact Admin");
            }
            return View(statusVM);
        }

        // Get /Status/Delete
        public ActionResult Delete(bool? saveChangesError = false, int ID = 0)
        {
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete Failed, Try Again";
            }
            Status status = statusRepo.FindBy(ID);
            return View(status);

        }
        //POST: /Status/Delete/3
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int ID)
        {
            try
            {
                Status status = statusRepo.FindBy(ID);
                statusRepo.Delete(status);
                statusRepo.Save();
            }
            catch (DataException)
            {
                return RedirectToAction("Delete", new { ID = ID, saveChangesError = true });
            }
            return RedirectToAction("Index");
        }


        protected override void Dispose(bool disposing)
        {
            statusRepo.Dispose();
            base.Dispose(disposing);
        }




    }
}