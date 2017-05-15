using Microsoft.AspNet.Identity;
using NotificationPortal.Models;
using NotificationPortal.Repositories;
using NotificationPortal.ViewModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace NotificationPortal.Api
{
    public class NotificationController : ApiController
    {
        private readonly NotificationApiRepo _nApiRepo = new NotificationApiRepo();
        
        // POST: api/Notification
        public NotificationIndexFiltered Post([FromBody] NotificationIndexBody model)
        {
            NotificationIndexFiltered result = _nApiRepo.GetFilteredAndSortedNotifications(model);
            return result;
        }
    }
}
