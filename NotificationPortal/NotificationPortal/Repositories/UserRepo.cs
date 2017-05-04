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

        public UserDeleteVM GetDeleteUser(string referenceId)
        {
            UserDeleteVM userToBeDeleted = _context.UserDetail.Where(u => u.ReferenceID == referenceId)
                                           .Select(user => new UserDeleteVM()
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

            return userToBeDeleted;
        }

        public bool DeleteUser(string referenceId, string clientName, out string msg)
        {
            UserDetail userToBeDeleted = _context.UserDetail.FirstOrDefault(u => u.ReferenceID == referenceId);

            var appUserTobeDeleted = _context.Users.FirstOrDefault(u => u.Id == userToBeDeleted.UserID);

            Client clientToBeDeleted = _context.Client.FirstOrDefault(c => c.ClientName == clientName);

            //Application clientApplication = _context.Application.FirstOrDefault(a => a.ApplicationID == clientToBeDeleted.ClientID);

            if (userToBeDeleted == null && appUserTobeDeleted == null && clientToBeDeleted == null)
            {
                msg = "User cannot be deleted.";

                return false;
            }

            //if (clientApplication != null)
            //{
            //    msg = "User associated with application(s), cannot be deleted.";

            //    return false;
            //}

            try
            {
                _context.UserDetail.Remove(userToBeDeleted);
                _context.SaveChanges();

                _context.Users.Remove(appUserTobeDeleted);
                _context.SaveChanges();

                msg = "User deleted successfully!";

                return true;
            }
            catch
            {
                msg = "Failed to delete user.";

                return false;
            }
        }
    }
}