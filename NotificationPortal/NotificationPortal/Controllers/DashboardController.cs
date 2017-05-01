using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NotificationPortal.Repositories;
using NotificationPortal.ViewModels;

namespace NotificationPortal.Controllers
{
    public class DashboardController : Controller
    {
        private readonly DashboardRepo _dashboardRepo = new DashboardRepo();

        // GET: Dashboard
        [Authorize]
        public ActionResult Index()
        {
            IEnumerable<DashboardVM> dashboard = _dashboardRepo.GetDashBoard();

            return View(dashboard);
        }
    }
}