using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NotificationPortal.Models;
using NotificationPortal.ViewModels;

namespace NotificationPortal.Repositories
{
    public class UserRepo
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        public IEnumerable<UserVM> GetAll()
        {
            IEnumerable<UserVM> users = _context.UserDetail.Where(u => u.Status.StatusID == u.StatusID)
                                        .Select(user => new UserVM()
                                        {
                                            UserID = user.UserID,
                                            FirstName = user.FirstName,
                                            LastName = user.LastName,
                                            BusinessTitle = user.BusinessTitle,
                                            BusinessPhone = user.BusinessPhone,
                                            MobilePhone = user.MobilePhone,
                                            HomePhone = user.HomePhone,
                                            ClientID = user.ClientID,
                                            StatusID = user.Status.StatusID,
                                            StatusName = user.Status.StatusName
                                        });

            return users;
        }

        public IEnumerable<SelectListItem> GetStatusList()
        {
            IEnumerable<SelectListItem> statusList = _context.Status.Where(s => s.StatusType.StatusTypeName == "User")
                                                     .Select(app => new SelectListItem
                                                     {
                                                         Value = app.StatusID.ToString(),
                                                         Text = app.StatusName
                                                     });

            return new SelectList(statusList, "Value", "Text");
        }

        public UserVM GetUserDetails(string id)
        {
            if (!_context.UserDetail.Any(o => o.UserID == id))
            {
                throw new HttpException(404, "Page Not Found");
            }

            var details = _context.UserDetail.Where(u => u.UserID == id)
                          .Select(user => new UserVM()
                          {
                              UserID = user.UserID,
                              FirstName = user.FirstName,
                              LastName = user.LastName,
                              BusinessTitle = user.BusinessTitle,
                              BusinessPhone = user.BusinessPhone,
                              MobilePhone = user.MobilePhone,
                              HomePhone = user.HomePhone,
                              ClientID = user.ClientID,
                              StatusID = user.Status.StatusID,
                          }).FirstOrDefault();

            return details;
        }
    }
}