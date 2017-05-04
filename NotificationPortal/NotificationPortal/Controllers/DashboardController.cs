using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NotificationPortal.Repositories;
using NotificationPortal.ViewModels;
using NotificationPortal.Models;

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
            //save user full name in session
            ApplicationDbContext _context = new ApplicationDbContext();
            string name = User.Identity.Name;
            UserDetail user = _context.UserDetail
                    .Where(u => u.User.UserName == name).FirstOrDefault();
            if (user != null) {
                string userFullName = user.FirstName + " " + user.LastName;
                Session["FullName"] = userFullName;
            }
            return View(dashboard);
        }
    }
}