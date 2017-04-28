using NotificationPortal.Models;
using NotificationPortal.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static NotificationPortal.ViewModels.ValidationVM;

namespace NotificationPortal.Controllers
{
    public class ClientController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();

        [Authorize]
        public ActionResult Index()
        {
            var statusTypeClient = context.StatusType.Where(s => s.StatusTypeName == "Client").Select(c => c.StatusTypeID).FirstOrDefault();
            var statusId = context.Client.Select(c => c.Status.StatusID).FirstOrDefault();
            var statusName = context.Status.Where(s => s.StatusTypeID == statusTypeClient && s.StatusID == statusId).FirstOrDefault();
            var clients = context.Client.ToList();

            List<ClientVM> clientList = new List<ClientVM>();

            foreach(var client in clients)
            {
                var user = new ClientVM()
                {
                    ClientName = client.ClientName,
                    StatusName = statusName.StatusName
                };

                clientList.Add(user);
            }
            
            return View(clientList);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(ClientVM model)
        {
            if(ModelState.IsValid)
            {

            }

            return View();
        }
    }
}