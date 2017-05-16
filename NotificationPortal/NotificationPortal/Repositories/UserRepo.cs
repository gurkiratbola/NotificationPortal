using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NotificationPortal.Models;
using NotificationPortal.ViewModels;
using System.Data.SqlClient;
using PagedList;

namespace NotificationPortal.Repositories
{
    public class UserRepo
    {
        // Get the database reference
        private readonly ApplicationDbContext _context = new ApplicationDbContext();
        // Get the selectlistrepo reference
        private readonly SelectListRepo _selectRepo = new SelectListRepo();

        // Sort the User Index page with table columns
        public IEnumerable<UserVM> Sort(IEnumerable<UserVM> list, string sortOrder, string searchString = null)
        {
            // Check if search is not null then search the database for query
            if (!String.IsNullOrEmpty(searchString))
            {
                list = list.Where(c => c.FirstName.ToUpper().Contains(searchString.ToUpper()) || c.RoleName.ToUpper().Contains(searchString.ToUpper()) || c.StatusName.ToUpper().Contains(searchString.ToUpper()) || c.LastName.ToUpper().Contains(searchString.ToUpper()) || c.Email.ToUpper().Contains(searchString.ToUpper()));
            }

            // Sort the different table columns on whichever one was selected
            switch (sortOrder)
            {
                case ConstantsRepo.SORT_ROLE_NAME_BY_DESC:
                    list = list.OrderByDescending(c => c.RoleName);
                    break;
                case ConstantsRepo.SORT_EMAIL_BY_DESC:
                    list = list.OrderByDescending(c => c.Email);
                    break;
                case ConstantsRepo.SORT_EMAIL_BY_ASCE:
                    list = list.OrderBy(c => c.Email);
                    break;
                case ConstantsRepo.SORT_FIRST_NAME_BY_DESC:
                    list = list.OrderByDescending(c => c.FirstName);
                    break;
                case ConstantsRepo.SORT_FIRST_NAME_BY_ASCE:
                    list = list.OrderBy(c => c.FirstName);
                    break;
                case ConstantsRepo.SORT_LAST_NAME_BY_DESC:
                    list = list.OrderByDescending(c => c.LastName);
                    break;
                case ConstantsRepo.SORT_LAST_NAME_BY_ASCE:
                    list = list.OrderBy(c => c.LastName);
                    break;
                case ConstantsRepo.SORT_CLIENT_BY_NAME_DESC:
                    list = list.OrderByDescending(c => c.ClientName);
                    break;
                case ConstantsRepo.SORT_CLIENT_BY_NAME_ASCE:
                    list = list.OrderBy(c => c.ClientName);
                    break;
                case ConstantsRepo.SORT_STATUS_BY_NAME_DESC:
                    list = list.OrderByDescending(c => c.StatusName);
                    break;
                case ConstantsRepo.SORT_STATUS_BY_NAME_ASCE:
                    list = list.OrderBy(c => c.StatusName);
                    break;
                default:
                    list = list.OrderBy(c => c.RoleName);
                    break;
            }

            return list;
        }

        // User Controller Index Page
        // Get the users and sort them
        public UserIndexVM GetAllUsers(string sortOrder, string currentFilter, string searchString, int? page)
        {
            // get the role manager to find user's role and populate the index page
            var roleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(_context));

            // get the current logged in user's id
            var currentUserId = HttpContext.Current.User.Identity.GetUserId();
            // get the current logged in user's client id
            var getClientId = _context.UserDetail.Where(u => u.UserID == currentUserId).Select(c => c.Client.ClientName).FirstOrDefault();

            try
            {
                // store the user information in list by using userVM view model
                IEnumerable<UserVM> users = _context.Users.Select(user => new UserVM()
                {
                    ReferenceID = user.UserDetail.ReferenceID,
                    Email = user.Email,
                    // get the role id associated to the user
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
                    // order the list by status 
                }).OrderByDescending(s => s.StatusID).ToList();

                // loop the users list and find the role name associated with the id and assign it to RoleName
                foreach (var user in users)
                {
                    user.RoleName = roleManager.FindById(user.RoleName).Name.ToString();
                }

                // If current logged in user is staff, do not show admin and staff users
                if (HttpContext.Current.User.IsInRole(Key.ROLE_STAFF))
                {
                    users = users.Where(a => a.RoleName != Key.ROLE_ADMIN && a.RoleName != Key.ROLE_STAFF);
                }

                // If current logged in user is client, do not show admin, staff and other clients including itself and where clientname matches itself
                if (HttpContext.Current.User.IsInRole(Key.ROLE_CLIENT))
                {
                    users = users.Where(u => u.RoleName != Key.ROLE_ADMIN && u.RoleName != Key.ROLE_STAFF && u.RoleName != Key.ROLE_CLIENT && u.ClientName == getClientId);
                }

                // get the current users in the database count
                int totalNumOfUsers = users.Count();

                // initilize the sorting, searching and filtering options
                page = searchString == null ? page : 1;
                int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
                searchString = searchString ?? currentFilter;
                int pageNumber = (page ?? 1);
                int defaultPageSize = ConstantsRepo.PAGE_SIZE;

                // sort by status name by default
                sortOrder = sortOrder == null ? ConstantsRepo.SORT_STATUS_BY_NAME_DESC : sortOrder;

                // store the users information info ipagedlist userindexvm viewmodel and sort it
                UserIndexVM model = new UserIndexVM
                {
                    Users = Sort(users, sortOrder, searchString).ToPagedList(pageNumber, defaultPageSize),
                    CurrentFilter = searchString,
                    CurrentSort = sortOrder,
                    TotalItemCount = totalNumOfUsers,
                    ItemStart = currentPageIndex * 10 + 1,
                    ItemEnd = totalNumOfUsers - (10 * currentPageIndex) >= 10 ? 10 * (currentPageIndex + 1) : totalNumOfUsers,
                    // get the properties from the viewmodel and assign the sorting parameters to them
                    RoleNameSort = sortOrder == ConstantsRepo.SORT_ROLE_NAME_BY_DESC ? ConstantsRepo.SORT_ROLE_NAME_BY_ASCE : ConstantsRepo.SORT_ROLE_NAME_BY_DESC,
                    EmailSort = sortOrder == ConstantsRepo.SORT_EMAIL_BY_DESC ? ConstantsRepo.SORT_EMAIL_BY_ASCE : ConstantsRepo.SORT_EMAIL_BY_DESC,
                    ClientHeadingSort = sortOrder == ConstantsRepo.SORT_CLIENT_BY_NAME_DESC ? ConstantsRepo.SORT_CLIENT_BY_NAME_ASCE : ConstantsRepo.SORT_CLIENT_BY_NAME_DESC,
                    FirstNameSort = sortOrder == ConstantsRepo.SORT_FIRST_NAME_BY_DESC ? ConstantsRepo.SORT_FIRST_NAME_BY_ASCE : ConstantsRepo.SORT_FIRST_NAME_BY_DESC,
                    LastNameSort = sortOrder == ConstantsRepo.SORT_LAST_NAME_BY_DESC ? ConstantsRepo.SORT_LAST_NAME_BY_ASCE : ConstantsRepo.SORT_LAST_NAME_BY_DESC,
                    StatusSort = sortOrder == ConstantsRepo.SORT_STATUS_BY_NAME_DESC ? ConstantsRepo.SORT_STATUS_BY_NAME_ASCE : ConstantsRepo.SORT_STATUS_BY_NAME_DESC,
                };

                return model;
            }
            // if users list has no users return null, which will display no users in the index view
            catch (Exception e)
            {
                if (e is SqlException)
                { }

                return null;
            }
        }

        // User Controller Details Page
        // Find the user by the passed in reference id
        public UserVM GetUserDetails(string id)
        {
            try
            {
                // get the rolemanager to find the user role
                var roleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(_context));

                // get the user details associated with the reference id and populate the information into UserVM view model
                var details = _context.Users.Where(u => u.UserDetail.ReferenceID == id)
                              .Select(user => new UserVM()
                              {
                                  ReferenceID = user.UserDetail.ReferenceID,
                                  Email = user.Email,
                                  // find the role id and assign name later
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
                                  // get the applications associated with the user and populate them into applicationvm
                                  Applications = user.UserDetail.Applications.Select(a => new ApplicationVM()
                                  {
                                      ApplicationName = a.ApplicationName,
                                      Description = a.Description,
                                      URL = a.URL,
                                      ReferenceID = a.ReferenceID
                                  })
                              }).FirstOrDefault();

                // get the role name from the role id and assign to RoleName property
                details.RoleName = roleManager.FindById(details.RoleName).Name.ToString();

                // initialize the dropdown / selectlistrepo values to be used for other pages
                details.StatusList = _selectRepo.GetStatusList(Key.STATUS_TYPE_USER);
                details.ClientList = _selectRepo.GetUserClientList();
                details.RoleList = _selectRepo.GetRolesList();
                details.ApplicationList = GetApplicationList();

                return details;
            }
            catch (Exception e)
            {
                if (e is SqlException)
                { }

                // if no user found with that specific reference id return null and handle it in the controller by displaying error message 
                return null;
            }
        }

        // User Controller Add Page
        public bool AddUser(AddUserVM model, out string msg, out string userId)
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

                    // find the client id with the reference id passed with the viewmodel and add the new client to that
                    var clientId = _context.Client.Where(c => c.ReferenceID == model.ClientReferenceID)
                                   .Select(client => client.ClientID).FirstOrDefault();

                    // get default send method (Email)
                    int defaultSendMethodID = _context.SendMethod.Where(s => s.SendMethodName == Key.SEND_METHOD_EMAIL)
                                              .Select(s => s.SendMethodID).FirstOrDefault();

                    // duplicate the user from aspnetuser to userdetail
                    UserDetail details = new UserDetail()
                    {
                        UserID = user.Id,
                        ReferenceID = Guid.NewGuid().ToString(),
                        BusinessTitle = model.BusinessTitle,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        StatusID = model.StatusID,
                        ClientID = clientId,
                        SendMethodID = defaultSendMethodID
                    };

                    // check what role the user is adding otherwise throw error
                    // staff can only add client and user roles not admin and other staff
                    if (HttpContext.Current.User.IsInRole(Key.ROLE_STAFF))
                    {
                        if (model.RoleName == Key.ROLE_ADMIN || model.RoleName == Key.ROLE_STAFF)
                        {
                            msg = "Something went wrong, try again";
                            userId = "";

                            return false;
                        }
                    }

                    // clients can only add other users, if false throw error
                    if (HttpContext.Current.User.IsInRole(Key.ROLE_CLIENT))
                    {
                        if (model.RoleName == Key.ROLE_ADMIN || model.RoleName == Key.ROLE_STAFF || model.RoleName == Key.ROLE_CLIENT)
                        {
                            msg = "Something went wrong, try again";
                            userId = "";

                            return false;
                        }
                    }
                    
                    // get the application reference ids from the multiselect
                    if (model.ApplicationReferenceIDs == null)
                    {
                        model.ApplicationReferenceIDs = new string[0];
                    }

                    // add the applications selected to user details application table
                    var apps = _context.Application.Where(a => model.ApplicationReferenceIDs.Contains(a.ReferenceID));

                    details.Applications = apps.ToList();

                    // make the user at this point
                    userManager.Create(user);

                    // pass the userId to controller to send the email
                    userId = user.Id;
                    // add the user details to the database 
                    _context.UserDetail.Add(details);
                    _context.SaveChanges();

                    // verify which role the user needs to be added     
                    userManager.AddToRole(user.Id, model.RoleName);

                    // if the client was added successfully pass this msg out
                    msg = "User added successfully";
                    return true;
                }
                else
                {
                    // if error show this msg
                    msg = "The email address is already in use";
                    userId = "";
                    return false;
                }
            }
            catch (Exception e)
            {
                if (e is SqlException)
                { }
                
                // if something breaks show this message or exception is caught
                msg = "Failed to add the user";
                userId = "";
                return false;
            }
        }

        // User Controller Edit Page
        public bool EditUser(UserVM model, out string msg)
        {
            try
            {
                // find the user detail with the reference id passed
                UserDetail user = _context.UserDetail.FirstOrDefault(u => u.ReferenceID == model.ReferenceID);

                // update the information to the client table as well by getting the client reference id
                var clientId = _context.Client.Where(c => c.ReferenceID == model.ClientReferenceID)
                               .Select(client => client.ClientID).FirstOrDefault();

                // if user is not null then proceed
                if (user != null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.BusinessTitle = model.BusinessTitle;
                    user.BusinessPhone = model.BusinessPhone;
                    user.MobilePhone = model.MobilePhone;
                    user.HomePhone = model.HomePhone;
                    user.ClientID = clientId;
                    user.StatusID = model.StatusID;

                    // Get the user manager 
                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));

                    // find the old role and user id and compare
                    var userId = userManager.FindById(user.UserID);
                    var identityUserRole = userId.Roles.SingleOrDefault();
                    if (identityUserRole != null)
                    {
                        var oldRoleId = identityUserRole.RoleId;
                        IdentityRole singleOrDefault = _context.Roles.SingleOrDefault(a => a.Id == oldRoleId);

                        if (singleOrDefault != null)
                        {
                            var oldRoleName = singleOrDefault.Name;

                            // if the current user is in staff role they cannot edit other users to staff and admin
                            if (HttpContext.Current.User.IsInRole(Key.ROLE_STAFF))
                            {
                                if (model.RoleName == Key.ROLE_ADMIN || model.RoleName == Key.ROLE_STAFF)
                                {
                                    // break at this point and show error message
                                    msg = "Something went wrong, try again";
                                    return false;
                                }
                            }

                            // if the current user is in client role they cannot edit other users to staff, client and user
                            if (HttpContext.Current.User.IsInRole(Key.ROLE_CLIENT))
                            {
                                if (model.RoleName == Key.ROLE_ADMIN || model.RoleName == Key.ROLE_STAFF || model.RoleName == Key.ROLE_CLIENT)
                                {
                                    // break at this point and show error message
                                    msg = "Something went wrong, try again";
                                    return false;
                                }
                            }

                            // update the user role only IF the current role assigned is not equal to the previous one
                            if (oldRoleName != model.RoleName)
                            {
                                userManager.RemoveFromRole(user.UserID, oldRoleName);
                                userManager.AddToRole(user.UserID, model.RoleName);
                            }
                        }
                    }

                    // check to see if the updated email exists in the database otherwise break and throw error message
                    var checkEmailExists = _context.Users.Where(e => e.Email == model.Email && e.UserName == model.Email).FirstOrDefault();

                    if (checkEmailExists != null && user.UserID == checkEmailExists.Id)
                    {
                        userId.Email = model.Email;
                        userId.UserName = model.Email;
                    }
                    else
                    {
                        msg = "User with the email address already exists";
                        return false;
                    }

                    // check for previous applications and remove it
                    if (model.ApplicationReferenceIDs == null)
                    {
                        model.ApplicationReferenceIDs = new string[0];
                    }

                    var apps = _context.Application.Where(a => model.ApplicationReferenceIDs.Contains(a.ReferenceID));

                    // clear all the applications associated with the user 
                    user.Applications.Clear();
                    // add new applications to the user
                    user.Applications = apps.ToList();

                    // _context.Entry(model).State = EntityState.Modified;
                    _context.SaveChanges();

                    msg = "User information successfully updated";
                    return true;
                }

                // if user does not exist break at this point
                msg = "Failed to update the user";
                return false;
            }
            catch (Exception e)
            {
                if (e is SqlException)
                { }

                // handle the exceptions whether its regular exception or sqlexception
                msg = "Failed to update the user";
                return false;
            }
        }

        // User Controller GET: Delete Page
        public UserDeleteVM GetDeleteUser(string referenceId)
        {
            // get the user information from the passed in reference id
            try
            {
                // get the role manager to get the user role
                var roleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(_context));

                // load the values into the UserDeleteVM 
                UserDeleteVM userToBeDeleted = _context.Users.Where(u => u.UserDetail.ReferenceID == referenceId)
                                               .Select(user => new UserDeleteVM()
                                               {
                                                   ReferenceID = user.UserDetail.ReferenceID,
                                                   Email = user.Email,
                                                   // find the role id associated to the user
                                                   RoleName = user.Roles.FirstOrDefault().RoleId.ToString(),
                                                   // find the client id associated to the user
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
                                                   // get the applications associated with the user to be deleted
                                                   Applications = user.UserDetail.Applications.Select(a => new ApplicationVM()
                                                   {
                                                       ApplicationName = a.ApplicationName,
                                                       Description = a.Description,
                                                       URL = a.URL,
                                                       ReferenceID = a.ReferenceID
                                                   })
                                               }).FirstOrDefault();

                // get the actual role name assigned the user from the role id above
                userToBeDeleted.RoleName = roleManager.FindById(userToBeDeleted.RoleName).Name.ToString();

                return userToBeDeleted;
            }
            catch (Exception e)
            {
                if (e is SqlException)
                { }

                // if no user found return null and handle it in the controller
                return null;
            }
        }

        // User Controller POST: Delete
        public bool DeleteUser(string referenceId, string clientReferenceId, out string msg)
        {
            // get the user by the reference id passed
            UserDetail userToBeDeleted = _context.UserDetail.FirstOrDefault(u => u.ReferenceID == referenceId);

            // find the same user in the ASPNETUSERS table
            var appUserTobeDeleted = _context.Users.FirstOrDefault(u => u.Id == userToBeDeleted.UserID);

            // find the client associated with the user
            Client clientToBeDeleted = _context.Client.FirstOrDefault(c => c.ReferenceID == clientReferenceId);

            // check if the user actually exists
            if (userToBeDeleted == null && appUserTobeDeleted == null && clientToBeDeleted == null)
            {
                msg = "User cannot be deleted";
                return false;
            }

            try
            {
                // only proceed if the current logged in user is not deleting itself, otherwise break and show error message
                if (userToBeDeleted != null && HttpContext.Current.User.Identity.GetUserId() != userToBeDeleted.UserID)
                {
                    // delete the user from user details
                    _context.UserDetail.Remove(userToBeDeleted);
                    _context.SaveChanges();

                    // delete the user from aspnetusers tbale
                    _context.Users.Remove(appUserTobeDeleted);
                    _context.SaveChanges();

                    // show success page if it went ok
                    msg = "User deleted successfully";
                    return true;
                }

                // show message that current logged in user cannot delete itself
                msg = "User cannot delete own account, Please contact an admin to delete your account";
                return false;
            }
            catch (Exception e)
            {
                if (e is SqlException)
                { }

                // if something breaks show this error and catch all the exceptions
                msg = "Failed to delete user";
                return false;
            }
        }

        // This is for the multiselect Application part for Add / Edit User Controller
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
    }
}