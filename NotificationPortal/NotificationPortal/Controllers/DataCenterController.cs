using NotificationPortal.Models;
using NotificationPortal.Repositories;
using NotificationPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NotificationPortal.Controllers
{
    [Authorize(Roles = Key.ROLE_ADMIN + "," + Key.ROLE_STAFF)]
    public class DataCenterController : AppBaseController
    {
        ApplicationDbContext _context = new ApplicationDbContext();

        private readonly DataCenterRepo _sRepo = new DataCenterRepo();
        private readonly SelectListRepo _lRepo = new SelectListRepo();

        // GET: DataCenter
        public ActionResult Index()
        {
            IEnumerable<DataCenterVM> dataCenterList = _sRepo.GetDataCenterList();
            return View(dataCenterList);
        }
        [HttpGet]
        public ActionResult Create()
        {
            // To be modified: global method for status in development
            var model = new DataCenterVM
            {

                Servers = _lRepo.GetServerList()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DataCenterVM model)
        {
            string msg = "";
            if (ModelState.IsValid)
            {
                bool success = _sRepo.AddDataCenter(model, out msg);
                if (success)
                {
                    TempData["SuccessMsg"] = msg;
                    return RedirectToAction("index");
                }
                else
                {
                    TempData["ErrorMsg"] = msg;
                }
            }
            else
            {
                TempData["ErrorMsg"] = "Client cannot be added at this time.";
            }
            model.Servers = _lRepo.GetServerList();
            return View(model);
        }
    }
}