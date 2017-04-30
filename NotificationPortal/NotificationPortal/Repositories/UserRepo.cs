using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NotificationPortal.Models;
using NotificationPortal.ViewModels;

namespace NotificationPortal.Repositories
{
    public class UserRepo
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

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

        public IEnumerable<UserVM> GetAllUsers()
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

        public bool AddUser(AddUserVM model, out string msg)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
            ApplicationUser checkUser = userManager.Users.FirstOrDefault(u => u.Email.ToLower() == model.Email.ToLower());

            try
            {
                if (checkUser == null)
                {
                    var user = new ApplicationUser()
                    {
                        UserName = model.Email,
                        Email = model.Email
                    };

                    userManager.Create(user);

                    UserDetail details = new UserDetail()
                    {
                        UserID = user.Id,
                        BusinessTitle = model.BusinessTitle,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        StatusID = model.StatusID,
                    };

                    _context.UserDetail.Add(details);
                    _context.SaveChanges();

                    msg = "Client added successfully!";
                    return true;
                }
                else
                {
                    msg = "The email address is already in use.";

                    return false;
                }
            }
            catch
            {
                msg = "Failed to add the user!";
                return false;
            }
        }

        public bool EditUser(UserVM model, out string msg)
        {
            try
            {
                UserDetail user = _context.UserDetail.FirstOrDefault(u => u.UserID == model.UserID);

                if (user != null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.BusinessTitle = model.BusinessTitle;
                    user.BusinessPhone = model.BusinessPhone;
                    user.MobilePhone = model.MobilePhone;
                    user.HomePhone = model.HomePhone;
                    user.StatusID = model.StatusID;

                    _context.SaveChanges();
                    msg = "User information successfully updated!";

                    return true;
                }

                msg = "Failed to update the user.";

                return false;
            }
            catch
            {
                msg = "Failed to update the user.";

                return false;
            }
        }
    }
}