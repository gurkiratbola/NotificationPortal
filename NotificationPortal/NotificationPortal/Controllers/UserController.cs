using System.Web;
using System.Web.Mvc;
using NotificationPortal.Models;
using NotificationPortal.Repositories;
using NotificationPortal.ViewModels;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using NotificationPortal.Service;

namespace NotificationPortal.Controllers
{
    public class UserController : AppBaseController
    {
        // Reference to usermanager
        private ApplicationUserManager _userManager;

        private readonly UserRepo _userRepo = new UserRepo();
        private readonly SelectListRepo _selectRepo = new SelectListRepo();

        // Get the usermanager to send emails asynchronously, from Accounts Controller 
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        
        [Authorize(Roles = Key.ROLE_ADMIN + ", " + Key.ROLE_STAFF + ", " + Key.ROLE_CLIENT)]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            // Impmenting sort functionality to the index page, referenced from user repo
            UserIndexVM model = _userRepo.GetAllUsers(sortOrder, currentFilter, searchString, page);

            return View(model);
        }
        
        [Authorize(Roles = Key.ROLE_ADMIN + ", " + Key.ROLE_STAFF + ", " + Key.ROLE_CLIENT)]
        [HttpGet]
        public ActionResult Create()
        {
            // Load the dropdown / select list from the selectlistrepo to populate the fields
            var model = new AddUserVM()
            {
                StatusList = _selectRepo.GetStatusList(Key.STATUS_TYPE_USER),
                ClientList = _selectRepo.GetUserClientList(),
                RolesList = _selectRepo.GetRolesList(),
                ApplicationList = _selectRepo.GetApplicationListByClient(null)
        };
            return View(model);
        }
        
        // This action is async because we are sending emails asynchronously from App_Start/IdentityConfig.cs
        [Authorize(Roles = Key.ROLE_ADMIN + ", " + Key.ROLE_STAFF + ", " + Key.ROLE_CLIENT)]
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AddUserVM model)
        {
            // If success then proceed
            if (ModelState.IsValid)
            {
                // If the method returns true from user repo, do this
                if (_userRepo.AddUser(model, out string msg, out string userId))
                {
                    // Generate the email confirmation code for the user being generated
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(userId);

                    // Generating the call back url which will be sent to the user for confirmation
                    var callbackUrl = Url.Action("ConfirmEmail", "User", new { userId = userId, code = code }, protocol: Request.Url.Scheme);

                    // This is the content for the body of confirmation email message
                    string body = "Welcome to Notification Portal. In order to get started, you need to confirm your email address.";

                    // Send the confirmation email with TemplateService -> AccountEmail method
                    await UserManager.SendEmailAsync(userId, "Confirm your account", TemplateService.AccountEmail(callbackUrl, body, "Confirm Email"));

                    TempData["SuccessMsg"] = msg;
                    return RedirectToAction("Index");
                }

                TempData["ErrorMsg"] = msg;
            }

            // Load the dropdown / selectlistrepo values if adding the user fails
            model.StatusList = _selectRepo.GetStatusList(Key.STATUS_TYPE_USER);
            model.ClientList = _selectRepo.GetUserClientList();
            model.RolesList = _selectRepo.GetRolesList();
            model.ApplicationList = _selectRepo.GetApplicationListByClient(null);

            return View(model);
        }
        
        [Authorize(Roles = Key.ROLE_ADMIN + ", " + Key.ROLE_STAFF + ", " + Key.ROLE_CLIENT)]
        [HttpGet]
        public ActionResult Edit(string id)
        {
            // Get the user information and populate the form fields which reference id
            var user = _userRepo.GetUserDetails(id);

            // if there were errors in getting the user direct to index page and show error
            if(user == null)
            {
                TempData["ErrorMsg"] = "Cannot edit this user at this time";
                return RedirectToAction("Index");
            }

            return View(user);
        }
        
        [Authorize(Roles = Key.ROLE_ADMIN + ", " + Key.ROLE_STAFF + ", " + Key.ROLE_CLIENT)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserVM model)
        {
            if (ModelState.IsValid)
            {
                // if modelstate validation and user repo edituser method succeeded then proceed
                if (_userRepo.EditUser(model, out string msg))
                {
                    TempData["SuccessMsg"] = msg;

                    return RedirectToAction("Index");
                }
                // if repository validation fails, show error msg 
                else
                {
                    TempData["ErrorMsg"] = msg;
                }
            }

            // load the dropdown / selectlistrepo values if editing the user fails
            model.StatusList = _selectRepo.GetStatusList(Key.STATUS_TYPE_USER);
            model.ClientList = _selectRepo.GetUserClientList();
            model.RoleList = _selectRepo.GetRolesList();
            model.ApplicationList = _selectRepo.GetApplicationListByClient(model.ClientReferenceID);

            return View(model);
        }
        
        [Authorize(Roles = Key.ROLE_ADMIN + ", " + Key.ROLE_STAFF + ", " + Key.ROLE_CLIENT)]
        [HttpGet]
        public ActionResult Details(string id)
        {
            // get the user details to populate the view the respective values
            var user = _userRepo.GetUserDetails(id);

            // if cannot find information redirect the user to index page and show error msg
            if(user == null)
            {
                TempData["ErrorMsg"] = "Cannot get this user at this time";
                return RedirectToAction("Index");
            }

            return View(user);
        }
    
        [Authorize(Roles = Key.ROLE_ADMIN + ", " + Key.ROLE_STAFF + ", " + Key.ROLE_CLIENT)]
        [HttpGet]
        public ActionResult Delete(string id)
        {
            // get the user reference id from the index page and populate the getdeleteuser method with information
            var user = _userRepo.GetDeleteUser(id);

            // if cannot find user show error msg and redirect back to index
            if(user == null)
            {
                TempData["ErrorMsg"] = "Cannot delete this user at this time";
                return RedirectToAction("Index");
            }

            return View(user);
        }
        
        [Authorize(Roles = Key.ROLE_ADMIN + ", " + Key.ROLE_STAFF + ", " + Key.ROLE_CLIENT)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(UserDeleteVM model)
        {
            if (ModelState.IsValid)
            {
                // proceed if the user repository deleteuser method succeeded
                if (_userRepo.DeleteUser(model.ReferenceID, model.ClientReferenceID, out string msg))
                {
                    TempData["SuccessMsg"] = msg;

                    return RedirectToAction("Index");
                }

                TempData["ErrorMsg"] = msg;
            }

            // load the view with same information if the deletion failed
            return View(_userRepo.GetDeleteUser(model.ReferenceID));
        }


        // This method is asynchronous to accept email confirmation because it was sent asynchronously from adduser
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            // Check if the required userId and code values are null 
            // Otherwise redirect to login page and show the error message
            if (userId == null || code == null)
            {
                TempData["ErrorMsg"] = "Something went wrong, please contact an admin.";
                return RedirectToAction("Login", "Account");
            }

            // If no errors at this point, confirm the email with usermanager
            var result = await UserManager.ConfirmEmailAsync(userId, code);

            // If there is an error confirming the email, redirect to login page with error message
            if (!result.Succeeded)
            {
                TempData["ErrorMsg"] = "Something went wrong, please contact an admin.";
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        // This method is asynchronous to accept email confirmation because it was sent asynchronously from adduser
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ConfirmEmail(SetPasswordVM model, string userId)
        {
            // check if there were any errors
            if (ModelState.IsValid)
            {
                // Find the user which email was confirmed in the GET method
                var user = await UserManager.FindByIdAsync(userId);

                // Check the database to check if the user exists and email is confirmed, if false redirect to different page with error msg
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    TempData["ErrorMsg"] = "Something went wrong, please contact an admin";
                    return RedirectToAction("ForgotPassword", "Account");
                }

                // Save the user's password from the model after email was confirmed and form submitted
                var result = await UserManager.AddPasswordAsync(user.Id, model.Password);

                // If above succeeded, redirect the user to login page 
                if (result.Succeeded)
                {
                    TempData["SuccessMsg"] = "Password set successfully, you may login";
                    return RedirectToAction("Login", "Account");
                }

                // If above failed, show error message on the current page
                TempData["ErrorMsg"] = "Something went wrong, please contact an admin";
            }

            return View(model);
        }
    }
}
