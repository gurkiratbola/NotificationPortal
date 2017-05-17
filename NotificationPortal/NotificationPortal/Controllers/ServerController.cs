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

        [HttpGet]
        public ActionResult Create()
        {
            // To be modified: global method for status in development
            var model = new ServerVM
            {
                StatusList = _serverRepo.GetStatusList(),
                LocationList = _serverRepo.GetLocationList(),
                ServerTypeList = _selectRepo.GetTypeList(),
                ApplicationList = _serverRepo.GetApplicationList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ServerVM model)
        {
            if (ModelState.IsValid)
            {
                if (_serverRepo.AddServer(model, out string msg))
                {
                    TempData["SuccessMsg"] = msg;
                    return RedirectToAction("index");
                }

                TempData["ErrorMsg"] = msg;
            }

            model.StatusList = _serverRepo.GetStatusList();
            model.LocationList = _serverRepo.GetLocationList();
            model.ServerTypeList = _serverRepo.GetServerTypeList();
            model.ApplicationList = _serverRepo.GetApplicationList();

            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            ServerVM server = _serverRepo.GetServer(id);

            // To be modified: global method for status in development
            server.StatusList = _serverRepo.GetStatusList();
            server.ServerTypeList = _serverRepo.GetServerTypeList();
            server.LocationList = _serverRepo.GetLocationList();
            server.ApplicationList = _serverRepo.GetApplicationList();

            return View(server);
        }

        [HttpPost]
        public ActionResult Edit(ServerVM model)
        {
            if (ModelState.IsValid)
            {
                if (_serverRepo.EditServer(model, out string msg))
                {
                    TempData["SuccessMsg"] = msg;
                    return RedirectToAction("Details", new { id = model.ReferenceID });
                }

                TempData["ErrorMsg"] = msg;
            }

            ServerVM server = _serverRepo.GetServer(model.ReferenceID);
            server.StatusList = _serverRepo.GetStatusList();
            server.LocationList = _serverRepo.GetLocationList();
            server.ServerTypeList = _serverRepo.GetServerTypeList();

            return View(server);
        }

        [HttpGet]
        public ActionResult Details(string id)
        {
            return View(_serverRepo.GetServerDetails(id));
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            return View(_serverRepo.GetDeleteServer(id));
        }

        [HttpPost]
        public ActionResult Delete(ServerDeleteVM server)
        {
            if (ModelState.IsValid)
            {
                if (_serverRepo.DeleteServer(server.ReferenceID, out string msg))
                {
                    TempData["SuccessMsg"] = msg;
                    return RedirectToAction("index");
                }

                TempData["ErrorMsg"] = msg;
            }

            return View(server);
        }
    }
}