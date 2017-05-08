using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NotificationPortal.Models;
using NotificationPortal.ViewModels;
using System.Data.SqlClient;
using System.Data.Entity;

namespace NotificationPortal.Repositories
{
    public class UserRepo
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        public IEnumerable<UserVM> Sort(IEnumerable<UserVM> list, string sortOrder, string searchString = null)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                list = list.Where(c => c.FirstName.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (sortOrder)
            {
                case ConstantsRepo.SORT_CLIENT_BY_NAME_DESC:
                    list = list.OrderByDescending(c => c.ClientName);
                    break;

                case ConstantsRepo.SORT_FIRST_NAME_BY_DESC:
                    list = list.OrderByDescending(c => c.FirstName);
                    break;

                case ConstantsRepo.SORT_FIRST_NAME_BY_ASCE:
                    list = list.OrderBy(c => c.FirstName);
                    break;

                default:
                    list = list.OrderBy(c => c.ClientName);
                    break;
            }
            return list;
        }

        public IEnumerable<UserVM> GetAllUsers()
        {
            try
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_context));

                var users = _context.Users.Select(user => new UserVM()
                {
                    ReferenceID = user.UserDetail.ReferenceID,
                    Email = user.Email,
                    RoleName = user.Roles.FirstOrDefault().RoleId.ToString(),
                    FirstName = user.UserDetail.FirstName,
                    LastName = user.UserDetail.LastName,
                    BusinessTitle = user.UserDetail.BusinessTitle,
                    BusinessPhone = user.UserDetail.BusinessPhone,
                    MobilePhone = user.UserDetail.MobilePhone,
                    HomePhone = user.UserDetail.HomePhone,
                    ClientReferenceID = user.UserDetail.Client.ReferenceID,
                    ClientName = user.UserDetail.Client.ClientName,
                    StatusID = user.UserDetail.Status.StatusID,
                    StatusName = user.UserDetail.Status.StatusName
                }).OrderByDescending(s => s.StatusID).ToList();

                foreach (var user in users)
                {
                    user.RoleName = roleManager.FindById(user.RoleName).Name.ToString();
                }

                return users;
            }
            catch (Exception e)
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

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_context));

            var details = _context.Users.Where(u => u.UserDetail.ReferenceID == id)
                          .Select(user => new UserVM()
                          {
                              ReferenceID = user.UserDetail.ReferenceID,
                              Email = user.Email,
                              RoleName = user.Roles.FirstOrDefault().RoleId.ToString(),
                              FirstName = user.UserDetail.FirstName,
                              LastName = user.UserDetail.LastName,
                              BusinessTitle = user.UserDetail.BusinessTitle,
                              BusinessPhone = user.UserDetail.BusinessPhone,
                              MobilePhone = user.UserDetail.MobilePhone,
                              HomePhone = user.UserDetail.HomePhone,
                              ClientReferenceID = user.UserDetail.Client.ReferenceID,
                              ClientName = user.UserDetail.Client.ClientName,
                              StatusID = user.UserDetail.Status.StatusID,
                              StatusName = user.UserDetail.Status.StatusName
                          }).FirstOrDefault();

            details.RoleName = roleManager.FindById(details.RoleName).Name.ToString();

            return details;
        }

        public bool AddUser(AddUserVM model, out string msg)
        {
            // Get the user manager 
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));

            // Check if the user exists in the database
            ApplicationUser checkUser = userManager.Users.FirstOrDefault(u => u.Email.ToLower() == model.Email.ToLower());

            try
            {
                // if user does not exist then ONLY proceed
                if (checkUser == null)
                {
                    // create a new AspNetUser
                    var user = new ApplicationUser()
                    {
                        UserName = model.Email,
                        Email = model.Email
                    };

                    // make the user at this point
                    userManager.Create(user);

                    // find the client id with the reference id passed with the viewmodel and add the new client to that
                    var clientID = _context.Client.Where(c => c.ReferenceID == model.ClientReferenceID)
                                   .Select(client => client.ClientID).FirstOrDefault();

                    // duplicate the user from aspnetuser to userdetail
                    UserDetail details = new UserDetail()
                    {
                        UserID = user.Id,
                        ReferenceID = Guid.NewGuid().ToString(),
                        BusinessTitle = model.BusinessTitle,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        StatusID = model.StatusID,
                        ClientID = clientID
                    };

                    // add the user details to the database 
                    _context.UserDetail.Add(details);
                    _context.SaveChanges();

                    // verify which role the user needs to be added     
                    userManager.AddToRole(user.Id, model.RoleName);

                    // if the client was added successfully pass this msg out
                    msg = "Client added successfully!";
                    return true;
                }
                else
                {
                    // if error show this msg
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

                var clientID = _context.Client.Where(c => c.ReferenceID == model.ClientReferenceID)
                               .Select(client => client.ClientID).FirstOrDefault();

                if (user != null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.BusinessTitle = model.BusinessTitle;
                    user.BusinessPhone = model.BusinessPhone;
                    user.MobilePhone = model.MobilePhone;
                    user.HomePhone = model.HomePhone;
                    user.ClientID = clientID;
                    user.StatusID = model.StatusID;

                    // Get the user manager 
                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
                    var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_context));

                    // find the old role and user id and compare
                    var userId = userManager.FindById(user.UserID);
                    var oldRoleId = userId.Roles.SingleOrDefault().RoleId;
                    var oldRoleName = _context.Roles.SingleOrDefault(a => a.Id == oldRoleId).Name;

                    if (oldRoleName != model.RoleName)
                    {
                        userManager.RemoveFromRole(user.UserID, oldRoleName);
                        userManager.AddToRole(user.UserID, model.RoleName);
                    }

                    var checkEmailExists = _context.Users.Where(e => e.Email == model.Email && e.UserName == model.Email).FirstOrDefault();

                    if(checkEmailExists == null)
                    {
                        userId.Email = model.Email;
                        userId.UserName = model.Email;
                    }
                    else
                    {
                        msg = "User with the email address already exists.";

                        return false;
                    }

                    // _context.Entry(model).State = EntityState.Modified;
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
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_context));

            UserDeleteVM userToBeDeleted = _context.Users.Where(u => u.UserDetail.ReferenceID == referenceId)
                                           .Select(user => new UserDeleteVM()
                                           {
                                               ReferenceID = user.UserDetail.ReferenceID,
                                               Email = user.Email,
                                               RoleName = user.Roles.FirstOrDefault().RoleId.ToString(),
                                               ClientReferenceID = user.UserDetail.Client.ReferenceID,
                                               FirstName = user.UserDetail.FirstName,
                                               LastName = user.UserDetail.LastName,
                                               BusinessTitle = user.UserDetail.BusinessTitle,
                                               BusinessPhone = user.UserDetail.BusinessPhone,
                                               MobilePhone = user.UserDetail.MobilePhone,
                                               HomePhone = user.UserDetail.HomePhone,
                                               ClientName = user.UserDetail.Client.ClientName,
                                               StatusID = user.UserDetail.Status.StatusID,
                                               StatusName = user.UserDetail.Status.StatusName
                                           }).FirstOrDefault();

            userToBeDeleted.RoleName = roleManager.FindById(userToBeDeleted.RoleName).Name.ToString();

            return userToBeDeleted;
        }

        public bool DeleteUser(string referenceId, string clientReferenceId, out string msg)
        {
            UserDetail userToBeDeleted = _context.UserDetail.FirstOrDefault(u => u.ReferenceID == referenceId);

            var appUserTobeDeleted = _context.Users.FirstOrDefault(u => u.Id == userToBeDeleted.UserID);

            Client clientToBeDeleted = _context.Client.FirstOrDefault(c => c.ReferenceID == clientReferenceId);

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

        public IEnumerable<ApplicationClientOptionVM> GetApplicationList()
        {
            var apps = _context.Application.Select(
            a => new ApplicationClientOptionVM
            {
                ApplicationName = a.ApplicationName,
                ReferenceID = a.ReferenceID,
                ClientReferenceID = a.Client.ReferenceID
            });

            return apps;
        }
    }
}
