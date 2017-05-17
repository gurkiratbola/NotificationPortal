using NotificationPortal.Models;
using NotificationPortal.Repositories;
using NotificationPortal.ViewModels;
using System.Web.Mvc;


namespace NotificationPortal.Controllers
{
    [Authorize(Roles = Key.ROLE_ADMIN + "," + Key.ROLE_STAFF)]
    public class ServerController : AppBaseController
    {
        private readonly ServerRepo _sRepo = new ServerRepo();
        private readonly SelectListRepo _lRepo = new SelectListRepo();


        [HttpGet]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ServerIndexVM model = _sRepo.GetServerList(sortOrder, currentFilter, searchString, page);
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            // To be modified: global method for status in development
            var model = new ServerVM
            {
                StatusList = _sRepo.GetStatusList(),
                LocationList = _sRepo.GetLocationList(),
                ServerTypeList = _lRepo.GetTypeList(),
                ApplicationList = _sRepo.GetApplicationList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ServerVM model)
        {
            if (ModelState.IsValid)
            {
                if (_sRepo.AddServer(model, out string msg))
                {
                    TempData["SuccessMsg"] = msg;
                    return RedirectToAction("index");
                }

                TempData["ErrorMsg"] = msg;
            }

            model.StatusList = _sRepo.GetStatusList();
            model.LocationList = _sRepo.GetLocationList();
            model.ServerTypeList = _sRepo.GetServerTypeList();
            model.ApplicationList = _sRepo.GetApplicationList();

            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            ServerVM server = _sRepo.GetServer(id);

            // To be modified: global method for status in development
            server.StatusList = _sRepo.GetStatusList();
            server.ServerTypeList = _sRepo.GetServerTypeList();
            server.LocationList = _sRepo.GetLocationList();
            server.ApplicationList = _sRepo.GetApplicationList();

            return View(server);
        }

        [HttpPost]
        public ActionResult Edit(ServerVM model)
        {
            if (ModelState.IsValid)
            {
                if (_sRepo.EditServer(model, out string msg))
                {
                    TempData["SuccessMsg"] = msg;
                    return RedirectToAction("Details", new { id = model.ReferenceID });
                }

                TempData["ErrorMsg"] = msg;
            }

            ServerVM server = _sRepo.GetServer(model.ReferenceID);
            server.StatusList = _sRepo.GetStatusList();
            server.LocationList = _sRepo.GetLocationList();
            server.ServerTypeList = _sRepo.GetServerTypeList();

            return View(server);
        }

        [HttpGet]
        public ActionResult Details(string id)
        {
            return View(_sRepo.GetDetailServer(id));
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            return View(_sRepo.GetDeleteServer(id));
        }

        [HttpPost]
        public ActionResult Delete(ServerDeleteVM server)
        {
            if (ModelState.IsValid)
            {
                if (_sRepo.DeleteServer(server.ReferenceID, out string msg))
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