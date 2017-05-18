using NotificationPortal.Models;
using NotificationPortal.Repositories;
using NotificationPortal.ViewModels;
using System.Web.Mvc;


namespace NotificationPortal.Controllers
{
    [Authorize(Roles = Key.ROLE_ADMIN + "," + Key.ROLE_STAFF)]
    public class ServerController : AppBaseController
    {
        private readonly ServerRepo _serverRepo = new ServerRepo();
        private readonly SelectListRepo _selectRepo = new SelectListRepo();

        [HttpGet]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ServerIndexVM model = _serverRepo.GetServerList(sortOrder, currentFilter, searchString, page);
            return View(model);
        }
        //Get Create new Server
        [HttpGet]
        public ActionResult Create()
        {
            // To be modified: global method for status in development
            var model = new ServerVM
            {
                StatusList = _serverRepo.GetStatusList(),
                LocationList = _serverRepo.GetLocationList(),
                ServerTypeList = _serverRepo.GetServerTypeList(),
                ApplicationList = _selectRepo.GetApplicationList()
            };

            return View(model);
        }
        //Post Create new server 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ServerVM model)
        {
            if (ModelState.IsValid)
            {
                if (_serverRepo.AddServer(model, out string msg))
                {
                    TempData["SuccessMsg"] = msg;
                    return RedirectToAction("Index");
                }

                TempData["ErrorMsg"] = msg;
            }

            model.StatusList = _serverRepo.GetStatusList();
            model.LocationList = _serverRepo.GetLocationList();
            model.ServerTypeList = _serverRepo.GetServerTypeList();
            model.ApplicationList = _selectRepo.GetApplicationList();

            return View(model);
        }
        //Get Edit Server
        [HttpGet]
        public ActionResult Edit(string id)
        {
            var server = _serverRepo.GetServerDetails(id);

            // To be modified: global method for status in development
            if (server == null)
            {
                TempData["ErrorMsg"] = "Cannot edit this server at this time";
                return RedirectToAction("Index");
            }

            return View(server);
        }
        //Post Edit server
        [HttpPost]
        public ActionResult Edit(ServerDetailVM model)
        {
            if (ModelState.IsValid)
            {
                if (_serverRepo.EditServer(model, out string msg))
                {
                    TempData["SuccessMsg"] = msg;
                    return RedirectToAction("Index");
                }

                TempData["ErrorMsg"] = msg;
            }

            model.StatusList = _serverRepo.GetStatusList();
            model.LocationList = _serverRepo.GetLocationList();
            model.ServerTypeList = _serverRepo.GetServerTypeList();
            model.ApplicationList = _selectRepo.GetApplicationList();

            return View(model);
        }
        //Get Server Detials
        [HttpGet]
        public ActionResult Details(string id)
        {
            var server = _serverRepo.GetServerDetails(id);

            if(server == null)
            {
                TempData["ErrorMsg"] = "Cannot get this server at this time";
                return RedirectToAction("Index");
            }

            return View(server);
        }
        //Post Server Delete
        [HttpGet]
        public ActionResult Delete(string id)
        {
            var server = _serverRepo.GetDeleteServer(id);

            if (server == null)
            {
                TempData["ErrorMsg"] = "Cannot delete this server at this time";
                return RedirectToAction("Index");
            }

            return View(server);
        }
        //Post Delete Server
        [HttpPost]
        public ActionResult Delete(ServerDeleteVM server)
        {
            if (ModelState.IsValid)
            {
                if (_serverRepo.DeleteServer(server.ReferenceID, out string msg))
                {
                    TempData["SuccessMsg"] = msg;
                    return RedirectToAction("Index");
                }

                TempData["ErrorMsg"] = msg;
            }

            return View(server);
        }
    }
}