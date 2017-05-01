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
    public class ServerController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();

        private IInterfaceRepo<Server> serverRepo;
        //[Authorize]
        public ServerController()
        {
            this.serverRepo = new ServerRepo(new ApplicationDbContext());
        }

        public ServerController(IInterfaceRepo<Server> serverRepo)
        {
            this.serverRepo = serverRepo;
        }

        ///GET /Servers
        public ViewResult Index()
        {
            var servers = from s in serverRepo.GetAll()
                          select s;
            return View(servers.ToList());
        }
        ///Get /Server/Detials/4
        public ViewResult Details(int ID)
        {

            Server server = serverRepo.FindBy(ID);
            return View(server);
        }
        //Get: /Server/Create
        public ActionResult Create()
        {
          
            ServerRepo serverRepo = new ServerRepo(new ApplicationDbContext());
            var model = new ServerVM
            {
                StatusList = serverRepo.GetStatusList(),
                LocationList = serverRepo.GetLocationList(),
            };
            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ServerName, Description,status,location")] ServerVM serverVM) {
        public ActionResult Create(ServerVM serverVM)
        {

            try
            {
                //test variables



                if (ModelState.IsValid)
                {
                ApplicationDbContext context = new ApplicationDbContext();
                ///get the location ID and status ID from here

                Server server = new Server();
                //server.LocationID = serverVM.location;
                server.LocationID = serverVM.LocationID;
                server.StatusID = serverVM.StatusID;
                server.Description = serverVM.Description;
                server.ServerName = serverVM.ServerName;
                server.ServerID = serverVM.ServerID;
                serverRepo.Add(server);
                serverRepo.Save();
                return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes. Contact Admin");
            }
            return View(serverVM);
        }



        public ActionResult Edit(int id)
        {

            ServerRepo serverRepo = new ServerRepo(new ApplicationDbContext());
            Server server = serverRepo.FindBy(id);
            var model = new ServerVM
            {
                StatusList = serverRepo.GetStatusList(),
                LocationList = serverRepo.GetLocationList(),
            };
            model.Description = server.Description;
            model.ServerName = server.ServerName;
          // serverVM.LocationList = location;
           //serverVM.StatusList = status;
            model.ServerID = server.ServerID;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ServerVM serverVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationDbContext context = new ApplicationDbContext();
                    ///get the location ID and status ID from here

                    Server server = serverRepo.FindBy(serverVM.ServerID);
                    //server.LocationID = serverVM.location;
                    server.LocationID = serverVM.LocationID;
                    server.StatusID = serverVM.StatusID;
                    server.Description = serverVM.Description;
                    server.ServerName = serverVM.ServerName;
                    server.ServerID = serverVM.ServerID;
                    serverRepo.Edit(server);
                    serverRepo.Save();
                    return RedirectToAction("Index");
                }
            
            }
            catch (DataException)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes. Contact Admin");
            }
            return View(Server);
        }
        // Get /Server/Delete
        public ActionResult Delete(bool? saveChangesError = false, int ID = 0)
        {
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete Failed, Try Again";
            }
            Server server = serverRepo.FindBy(ID);
            return View(server);

        }
        //POST: /Server/Delete/3
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int ID)
        {
            try
            {
                Server server = serverRepo.FindBy(ID);
                serverRepo.Delete(server);
                serverRepo.Save();
            }
            catch (DataException)
            {
                return RedirectToAction("Delete", new { ID = ID, saveChangesError = true });
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            serverRepo.Dispose();
            base.Dispose(disposing);
        }
    }
}