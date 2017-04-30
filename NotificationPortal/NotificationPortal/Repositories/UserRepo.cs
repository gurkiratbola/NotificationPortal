using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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


    }
}