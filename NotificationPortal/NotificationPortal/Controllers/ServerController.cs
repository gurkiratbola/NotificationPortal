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

            var location = context.DataCenterLocation.ToList();
            var status = context.Status.Where(e => e.StatusType.StatusTypeName == "server").ToList();
            var vModel = new ServerVM { status = status, location = location };    
            vModel.status = status;
            vModel.location = location;
            return View(vModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ServerName, Description,status,location")] ServerVM serverVM) {
        public ActionResult Create(ServerVM serverVM, int location, int status)
        {
           
            try
            {
                //test variables
               


                //if (ModelState.IsValid)
                //{
                    ApplicationDbContext context = new ApplicationDbContext();
                    ///get the location ID and status ID from here
                
                    Server server = new Server();
                    //server.LocationID = serverVM.location;
                    server.LocationID = location;
                    server.StatusID = status;
                    server.Description = serverVM.Description;
                    server.ServerName = serverVM.ServerName;
                    server.ServerID = serverVM.ServerID;
                    serverRepo.Add(server);
                    serverRepo.Save();
                    return RedirectToAction("Index");
                //}
            }
            catch (DataException)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes. Contact Admin");
            }
            return View(serverVM);
        }

        

        public ActionResult Edit(int id)
        {
            var location = context.DataCenterLocation.ToList();
            var status = context.Status.Where(e => e.StatusType.StatusTypeName == "server").ToList();
            Server server = serverRepo.FindBy(id);
            ServerVM serverVM = new ServerVM();
            serverVM.Description = server.Description;
            serverVM.ServerName = server.ServerName;
            serverVM.location = location;
            serverVM.status = status;
            serverVM.ServerID = server.ServerID;
            return View(serverVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int ServerID,string ServerName, string Description, int location, int status)
        {
            try
            {
                //if (ModelState.IsValid)
                //{
                ///get the location ID and status ID from here
                Server server = serverRepo.FindBy(ServerID);
                //server.LocationID = serverVM.location;
                    server.LocationID = location;
                    server.StatusID = status;
                    server.Description = Description;
                    server.ServerName = ServerName;
                    serverRepo.Edit(server);
                    serverRepo.Save();
                    return RedirectToAction("Index");
               // }
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