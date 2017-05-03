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
        const string APP_STATUS_TYPE_NAME = "User";
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        public IEnumerable<SelectListItem> GetStatusList()
        {
            IEnumerable<SelectListItem> statusList = _context.Status.Where(s => s.StatusType.StatusTypeName == APP_STATUS_TYPE_NAME)
                                                     .Select(app => new SelectListItem
                                                     {
                                                         Value = app.StatusID.ToString(),
                                                         Text = app.StatusName
                                                     });

            return new SelectList(statusList, "Value", "Text");
        }

        public IEnumerable<SelectListItem> GetClientList()
        {
            List<SelectListItem> clientList = _context.Client.Select(app => new SelectListItem
                                                     {
                                                         Value = app.ClientID.ToString(),
                                                         Text = app.ClientName
                                                     }).ToList();

            clientList.Add(new SelectListItem { Value = "-1", Text = "" });
            //clientList.OrderByDescending(x => x.Value);

            return new SelectList(clientList, "Value", "Text");
        }

        public IEnumerable<UserVM> GetAllUsers()
        {
            try
            {
                return _context.UserDetail.Where(u => u.Status.StatusID == u.StatusID)
                       .Select(user => new UserVM()
                       {
                           ReferenceID = user.ReferenceID,
                           FirstName = user.FirstName,
                           LastName = user.LastName,
                           BusinessTitle = user.BusinessTitle,
                           BusinessPhone = user.BusinessPhone,
                           MobilePhone = user.MobilePhone,
                           HomePhone = user.HomePhone,
                           ClientID = user.ClientID,
                           ClientName = user.Client.ClientName,
                           StatusID = user.Status.StatusID,
                           StatusName = user.Status.StatusName
                       }).OrderByDescending(s => s.StatusID);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public UserVM GetUserDetails(string id)
        {
            if (!_context.UserDetail.Any(o => o.ReferenceID == id))
            {
                throw new HttpException(404, "Page Not Found");
            }

            var details = _context.UserDetail.Where(u => u.ReferenceID == id)
                          .Select(user => new UserVM()
                          {
                              ReferenceID = user.ReferenceID,
                              FirstName = user.FirstName,
                              LastName = user.LastName,
                              BusinessTitle = user.BusinessTitle,
                              BusinessPhone = user.BusinessPhone,
                              MobilePhone = user.MobilePhone,
                              HomePhone = user.HomePhone,
                              ClientID = user.ClientID,
                              ClientName = user.Client.ClientName,
                              StatusID = user.Status.StatusID,
                              StatusName = user.Status.StatusName
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
                        ReferenceID = Guid.NewGuid().ToString(),
                        BusinessTitle = model.BusinessTitle,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        StatusID = model.StatusID,
                        ClientID = model.ClientId
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
                UserDetail user = _context.UserDetail.FirstOrDefault(u => u.ReferenceID == model.ReferenceID);

                if (user != null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.BusinessTitle = model.BusinessTitle;
                    user.BusinessPhone = model.BusinessPhone;
                    user.MobilePhone = model.MobilePhone;
                    user.HomePhone = model.HomePhone;
                    user.ClientID = model.ClientID;
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

        public void DeleteUser(string id, int? clientId, out string msg)
        {
            Client clientToBeDeleted = _context.Client.FirstOrDefault(c => c.ClientID == clientId);
            Application clientApplication = _context.Application.FirstOrDefault(a => a.ClientID == clientId);

            UserDetail userToBeDeleted = _context.UserDetail.FirstOrDefault(u => u.UserID == id);
            ApplicationUser appUserTobeDeleted = _context.Users.FirstOrDefault(u => u.Id == id);

            if (clientApplication != null)
            {
                msg = "User associated with application(s), cannot be deleted.";
            }
            else
            {
                if (clientToBeDeleted != null)
                {
                    msg = "User deleted successfully!";

                    if (userToBeDeleted != null)
                    {
                        _context.UserDetail.Remove(userToBeDeleted);
                        _context.SaveChanges();

                        msg = "User deleted successfully!";

                        if (appUserTobeDeleted != null)
                        {
                            _context.Users.Remove(appUserTobeDeleted);
                            _context.SaveChanges();
                            msg = "User deleted successfully!";
                        }
                    }
                }
                else
                {
                    msg = "Failed to delete user.";
                }
            }
        }
    }
}