using NotificationPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static NotificationPortal.ViewModels.ValidationVM;

namespace NotificationPortal.Repositories
{
    public class ClientRepo
    {
        private ApplicationDbContext context = new ApplicationDbContext();

        //public IEnumerable<Client> GetAll()
        //{
        //    var statusTypeClient = context.StatusType.Where(s => s.StatusTypeName == "Client").FirstOrDefault();


        //    return;
        //}
    }
}