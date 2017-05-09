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
using PagedList;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Security.Policy;

namespace NotificationPortal.Repositories
{
    public class UserRepo
    {
        const string EMAIL_CONFIRMATION = "EmailConfirmation";
        private readonly ApplicationDbContext _context = new ApplicationDbContext();
        private readonly SelectListRepo _selectRepo = new SelectListRepo();

        public IEnumerable<UserVM> Sort(IEnumerable<UserVM> list, string sortOrder, string searchString = null)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                list = list.Where(c => c.FirstName.ToUpper().Contains(searchString.ToUpper()) || c.RoleName.ToUpper().Contains(searchString.ToUpper()) || c.StatusName.ToUpper().Contains(searchString.ToUpper()) || c.LastName.ToUpper().Contains(searchString.ToUpper()) || c.Email.ToUpper().Contains(searchString.ToUpper()));
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

                case ConstantsRepo.SORT_STATUS_BY_NAME_DESC:
                    list = list.OrderByDescending(c => c.StatusName);
                    break;

                case ConstantsRepo.SORT_STATUS_BY_NAME_ASCE:
                    list = list.OrderBy(c => c.StatusName);
                    break;

                default:
                    list = list.OrderBy(c => c.ClientName);
                    break;
            }

            return list;
        }

        public UserIndexVM GetAllUsers(string sortOrder, string currentFilter, string searchString, int? page)
        {
            try
            {
                var roleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(_context));

                IEnumerable<UserVM> users = _context.Users.Select(user => new UserVM()
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

                int totalNumOfUsers = users.Count();
                page = searchString == null ? page : 1;
                int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
                searchString = searchString ?? currentFilter;
                int pageNumber = (page ?? 1);
                int defaultPageSize = ConstantsRepo.PAGE_SIZE;

                UserIndexVM model = new UserIndexVM
                {
                    Users = Sort(users, sortOrder, searchString).ToPagedList(pageNumber, defaultPageSize),
                    CurrentFilter = searchString,
                    CurrentSort = sortOrder,
                    TotalItemCount = totalNumOfUsers,
                    ItemStart = currentPageIndex * 10 + 1,
                    ItemEnd = totalNumOfUsers - (10 * currentPageIndex) >= 10 ? 10 * (currentPageIndex + 1) : totalNumOfUsers,
                    ClientHeadingSort = sortOrder == ConstantsRepo.SORT_CLIENT_BY_NAME_DESC ? ConstantsRepo.SORT_CLIENT_BY_NAME_ASCE : ConstantsRepo.SORT_CLIENT_BY_NAME_DESC,
                    FirstNameSort = sortOrder == ConstantsRepo.SORT_FIRST_NAME_BY_DESC ? ConstantsRepo.SORT_FIRST_NAME_BY_ASCE : ConstantsRepo.SORT_FIRST_NAME_BY_DESC,
                    StatusSort = sortOrder == ConstantsRepo.SORT_STATUS_BY_NAME_DESC ? ConstantsRepo.SORT_STATUS_BY_NAME_ASCE : ConstantsRepo.SORT_STATUS_BY_NAME_DESC,
                };

                return model;
            }
            catch (SqlException e)
            {
                return null;
            }
        }

        public UserVM GetUserDetails(string id)
        {
            //if (!_context.UserDetail.Any(o => o.ReferenceID == id))
            //{
            //    throw new HttpException(404, "Page Not Found");
            //}

            var roleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(_context));

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
                              StatusName = user.UserDetail.Status.StatusName,
                              Applications = user.UserDetail.Applications.Select(a=> new ApplicationVM()
                                             {
                                                ApplicationName = a.ApplicationName,
                                                Description = a.Description,
                                                URL = a.URL,
                                                ReferenceID = a.ReferenceID
                                             })
                          }).FirstOrDefault();

            details.RoleName = roleManager.FindById(details.RoleName).Name.ToString();

            details.StatusList = _selectRepo.GetStatusList(Key.STATUS_TYPE_USER);
            details.ClientList = _selectRepo.GetClientList();
            details.RoleList = _selectRepo.GetRolesList();
            details.ApplicationList = GetApplicationList();

            return details;
        }

        public bool AddUser(AddUserVM model, UrlHelper url, HttpRequestBase request, out string msg)
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
                    var result = userManager.Create(user);

                    if(result.Succeeded)
                    {
                        CreateTokenProvider(userManager, EMAIL_CONFIRMATION);

                        string verificationCode = userManager.GenerateEmailConfirmationToken(user.Id);

                        var callbackUrl = url.Action("ConfirmEmail", "Account", new { user.Id, code = verificationCode }, protocol: request.Url.Scheme);

                        var subject = "Confirm your account";
                        var message = "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>";
                        userManager.SendEmail(user.Id, subject, message);
                    }
                    else
                    {
                        msg = "Failed to add the user.";
                        return false;
                    }

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

                    if(model.ApplicationReferenceIDs == null)
                    {
                        model.ApplicationReferenceIDs = new string[0];
                    }

                    // add the applications selected to user details application table
                    var apps = _context.Application.Where(a => model.ApplicationReferenceIDs.Contains(a.ReferenceID));

                    details.Applications = apps.ToList();

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
            catch(Exception e)
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

                    if(checkEmailExists != null && user.UserID == checkEmailExists.Id)
                    {
                        userId.Email = model.Email;
                        userId.UserName = model.Email;
                    }
                    else
                    {
                        msg = "User with the email address already exists.";
                        return false;
                    }

                    // check for previous applications and remove it
                    if (model.ApplicationReferenceIDs == null)
                    {
                        model.ApplicationReferenceIDs = new string[0];
                    }

                    var apps = _context.Application.Where(a => model.ApplicationReferenceIDs.Contains(a.ReferenceID));

                    user.Applications.Clear();
                    user.Applications = apps.ToList();

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
            try
            {
                var roleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(_context));

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
                                                   StatusName = user.UserDetail.Status.StatusName,
                                                   Applications = user.UserDetail.Applications.Select(a => new ApplicationVM()
                                                   {
                                                       ApplicationName = a.ApplicationName,
                                                       Description = a.Description,
                                                       URL = a.URL,
                                                       ReferenceID = a.ReferenceID
                                                   })
                                               }).FirstOrDefault();

                userToBeDeleted.RoleName = roleManager.FindById(userToBeDeleted.RoleName).Name.ToString();

                return userToBeDeleted;
            }
            catch (Exception e)
            {
                if(e is SqlException)
                {

                }

                return null;
            }
        }

        public bool DeleteUser(string referenceId, string clientReferenceId, out string msg)
        {
            UserDetail userToBeDeleted = _context.UserDetail.FirstOrDefault(u => u.ReferenceID == referenceId);

            var appUserTobeDeleted = _context.Users.FirstOrDefault(u => u.Id == userToBeDeleted.UserID);

            Client clientToBeDeleted = _context.Client.FirstOrDefault(c => c.ReferenceID == clientReferenceId);

            if (userToBeDeleted == null && appUserTobeDeleted == null && clientToBeDeleted == null)
            {
                msg = "User cannot be deleted.";
                return false;
            }

            try
            {
                if(HttpContext.Current.User.Identity.GetUserId() != userToBeDeleted.UserID)
                {
                    _context.UserDetail.Remove(userToBeDeleted);
                    _context.SaveChanges();

                    _context.Users.Remove(appUserTobeDeleted);
                    _context.SaveChanges();

                    msg = "User deleted successfully!";
                    return true;
                }

                msg = "User cannot delete own account, Please contact an admin to delete your account.";
                return false;               
            }
            catch
            {
                msg = "Failed to delete user.";
                return false;
            }
        }

        public IEnumerable<ApplicationClientOptionVM> GetApplicationList()
        {
            var apps = _context.Application.Select(a => new ApplicationClientOptionVM
            {
                ApplicationName = a.ApplicationName,
                ReferenceID = a.ReferenceID,
                ClientReferenceID = a.Client.ReferenceID
            });

            return apps;
        }

        void CreateTokenProvider(UserManager<ApplicationUser> manager, string tokenType)
        {
            manager.UserTokenProvider = new EmailTokenProvider<ApplicationUser>();
        }
    }
}
