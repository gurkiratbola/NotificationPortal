using System.Web;
using System.Web.Mvc;
using NotificationPortal.Models;
using NotificationPortal.Repositories;
using NotificationPortal.ViewModels;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using System.Web.Security;
using NotificationPortal.Service;

namespace NotificationPortal.Controllers
{
    public class UserController : AppBaseController
    {
        private ApplicationUserManager _userManager;

        private readonly UserRepo _userRepo = new UserRepo();
        private readonly SelectListRepo _selectRepo = new SelectListRepo();

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

        // GET: UserDetails/Index
        [Authorize(Roles = Key.ROLE_ADMIN + ", " + Key.ROLE_STAFF + ", " + Key.ROLE_CLIENT)]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            UserIndexVM model = _userRepo.GetAllUsers(sortOrder, currentFilter, searchString, page);

            return View(model);
        }

        // GET: UserDetails/Add
        [Authorize(Roles = Key.ROLE_ADMIN + ", " + Key.ROLE_STAFF + ", " + Key.ROLE_CLIENT)]
        [HttpGet]
        public ActionResult Add()
        {
            var model = new AddUserVM()
            {
                StatusList = _selectRepo.GetStatusList(Key.STATUS_TYPE_USER),
                ClientList = _selectRepo.GetClientList(),
                RolesList = _selectRepo.GetRolesList(),
                ApplicationList = _userRepo.GetApplicationList()
            };

            return View(model);
        }

        // POST: UserDetails/Add
        [Authorize(Roles = Key.ROLE_ADMIN + ", " + Key.ROLE_STAFF + ", " + Key.ROLE_CLIENT)]
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(AddUserVM model)
        {
            if (ModelState.IsValid)
            {
                string msg = "";
                string userId = "";

                if (_userRepo.AddUser(model, out msg, out userId))
                {
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(userId);
                    var callbackUrl = Url.Action("ConfirmEmail", "User", new { userId = userId, code = code }, protocol: Request.Url.Scheme);
                    await UserManager.SendEmailAsync(userId, "Confirm your account", TemplateService.AccountEmail(callbackUrl));

                    TempData["SuccessMsg"] = msg;
                  
                    return RedirectToAction("Index");
                }

                TempData["ErrorMsg"] = msg;
            }

            model.StatusList = _selectRepo.GetStatusList(Key.STATUS_TYPE_USER);
            model.ClientList = _selectRepo.GetClientList();
            model.RolesList = _selectRepo.GetRolesList();
            model.ApplicationList = _userRepo.GetApplicationList();

            return View(model);
        }

        // GET: /User/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }            

            var result = await UserManager.ConfirmEmailAsync(userId, code);

            if (!result.Succeeded)
            {
                TempData["ErrorMsg"] = "Something went wrong, please contact an admin.";

                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        // POST: User/SetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ConfirmEmail(SetPasswordVM model, string userId)
        {
            if(ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(userId);

                if(user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    return RedirectToAction("ForgotPassword", "Account");
                }

                var result = await UserManager.AddPasswordAsync(user.Id, model.Password);

                if(result.Succeeded)
                {
                    TempData["SuccessMsg"] = "Password set successfully, you may login!";

                    return RedirectToAction("Login", "Account");
                }

                TempData["ErrorMsg"] = "Something went wrong, please contact an admin.";
            }

            return View(model);
        }

        // GET: UserDetails/Edit
        [Authorize(Roles = Key.ROLE_ADMIN + ", " + Key.ROLE_STAFF + ", " + Key.ROLE_CLIENT)]
        [HttpGet]
        public ActionResult Edit(string id)
        {
            return View(_userRepo.GetUserDetails(id));
        }

        // POST: UserDetails/Edit
        [Authorize(Roles = Key.ROLE_ADMIN + ", " + Key.ROLE_STAFF + ", " + Key.ROLE_CLIENT)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserVM model)
        {
            if (ModelState.IsValid)
            {
                string msg = "";

                if (_userRepo.EditUser(model, out msg))
                {
                    TempData["SuccessMsg"] = msg;

                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMsg"] = msg;
                }
            }

            model.StatusList = _selectRepo.GetStatusList(Key.STATUS_TYPE_USER);
            model.ClientList = _selectRepo.GetClientList();
            model.RoleList = _selectRepo.GetRolesList();
            model.ApplicationList = _userRepo.GetApplicationList();

            return View(model);
        }

        // GET: UserDetails/Details
        [Authorize(Roles = Key.ROLE_ADMIN + ", " + Key.ROLE_STAFF + ", " + Key.ROLE_CLIENT)]
        [HttpGet]
        public ActionResult Details(string id)
        {
            return View(_userRepo.GetUserDetails(id));
        }

        // GET: UserDetails/Delete
        [Authorize(Roles = Key.ROLE_ADMIN + ", " + Key.ROLE_STAFF + ", " + Key.ROLE_CLIENT)]
        [HttpGet]
        public ActionResult Delete(string id)
        {
            return View(_userRepo.GetDeleteUser(id));
        }

        // POST: UserDetails/Delete
        [Authorize(Roles = Key.ROLE_ADMIN + ", " + Key.ROLE_STAFF + ", " + Key.ROLE_CLIENT)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(UserDeleteVM model)
        {
            if (ModelState.IsValid)
            {
                var msg = "";
                if (_userRepo.DeleteUser(model.ReferenceID, model.ClientReferenceID, out msg))
                {
                    TempData["SuccessMsg"] = msg;

                    return RedirectToAction("Index");
                }

                TempData["ErrorMsg"] = msg;
            }

            //TempData["ErrorMsg"] = "User cannot be deleted at this time.";

            return View(_userRepo.GetDeleteUser(model.ReferenceID));
        }
    }
}
