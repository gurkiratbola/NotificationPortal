using NotificationPortal.Models;
using NotificationPortal.Repositories;
using NotificationPortal.Service;
using NotificationPortal.ViewModels;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NotificationPortal.Controllers
{
    [Authorize]
    public class NotificationController : AppBaseController
    {
        private readonly NotificationRepo _nRepo = new NotificationRepo();
        //public string GetTimeZoneOffset() {
        //    string timeOffsetString = "0";
        //    if (Request.Cookies["timezoneoffset"] != null)
        //    {
        //        timeOffsetString = Request.Cookies["timezoneoffset"].Value;
        //    }
        //    return timeOffsetString;
        //}

        [HttpGet]
        public ActionResult Index()
        {
            NotificationIndexVM model;
            model = _nRepo.CreateIndexModel();
            if (model==null)
            {
                // TODO: handle when model == null (something went wrong while querying the database)
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult DetailsThread(string id)
        {
            var model = _nRepo.CreateDetailModel(id);
            if (model == null)
            {
                TempData["ErrorMsg"] = "Cannot display this thread at the moment";
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = Key.ROLE_ADMIN + "," + Key.ROLE_STAFF)]
        public ActionResult CreateThread()
        {
            var model = _nRepo.CreateAddModel();
            if (model == null)
            {
                TempData["ErrorMsg"] = "Cannot create new thread at the moment";
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = Key.ROLE_ADMIN + "," + Key.ROLE_STAFF)]
        public async Task<ActionResult> CreateThread(NotificationCreateVM model) {
            string result = "";
            if (ModelState.IsValid)
            {
                // give this model a new incident #
                model.IncidentNumber = _nRepo.NewIncidentNumber(model.NotificationTypeID);
                // add notification to the database
                bool success = _nRepo.CreateNotification(model, out result);
                if (success)
                {
                    // send email and sms after notification has been created
                    await NotificationService.SendEmail(_nRepo.CreateMails(model));
                    await NotificationService.SendSMS(_nRepo.GetPhoneNumbers(model), TemplateService.NotificationSMS(model));

                    TempData["SuccessMsg"] = result;
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["ErrorMsg"] = "Cannot add Notification, model not valid.";
            }

            // recreate model for the view
            model = _nRepo.CreateAddModel(model);
            // if notification returns null, redirect to notification index
            if (model == null)
            {
                TempData["ErrorMsg"] = "Cannot create new thread at the moment";
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = Key.ROLE_ADMIN + "," + Key.ROLE_STAFF)]
        public ActionResult Create(string id)
        {
            var model = _nRepo.CreateUpdateModel(id);
            if (model == null)
            {
                TempData["ErrorMsg"] = "Cannot new notification at the moment";
                return RedirectToAction("DetailsThread", new { id = id });
            }
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = Key.ROLE_ADMIN + "," + Key.ROLE_STAFF)]
        public async Task<ActionResult> Create(NotificationCreateVM model)
        {
            string result = "";
            if (ModelState.IsValid)
            {
                // add notification to the database
                bool success = _nRepo.CreateNotification(model, out result);
                if (success)
                {
                    // send email and sms after notification has been created
                    await NotificationService.SendEmail(_nRepo.CreateMails(model));
                    await NotificationService.SendSMS(_nRepo.GetPhoneNumbers(model), TemplateService.NotificationSMS(model));

                    TempData["SuccessMsg"] = result;
                    return RedirectToAction("DetailsThread",new { id = model.IncidentNumber });
                }
            }
            else
            {
                ViewBag.ErrorMsg = "Cannot update Notification, model not valid.";
            }

            // recreate model for the view
            model = _nRepo.CreateUpdateModel(model.IncidentNumber, model);
            // if notification returns null, redirect to notification index
            if (model == null)
            {
                TempData["ErrorMsg"] = "Cannot create new notification at the moment";
                return RedirectToAction("Index");
            }
            return View(model);
        }
        
        [HttpGet]
        [Authorize(Roles = Key.ROLE_ADMIN + "," + Key.ROLE_STAFF)]
        public ActionResult Edit(string id)
        {
            var model = _nRepo.CreateEditModel(id);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = Key.ROLE_ADMIN + "," + Key.ROLE_STAFF)]
        public ActionResult Edit(NotificationEditVM model)
        {
            string result = "";
            if (ModelState.IsValid)
            {
                bool success = _nRepo.EditNotification(model, out result);
                if (success)
                {
                    TempData["SuccessMsg"] = result;
                    return RedirectToAction("DetailsThread", new { id = model.IncidentNumber });
                }
            }
            else
            {
                TempData["ErrorMsg"] = "Cannot edit Notification, model not valid.";
            }
            TempData["ErrorMsg"] = result;
            model = _nRepo.CreateEditModel(model.NotificationReferenceID, model);
            return View(model);
        }
        
        [HttpGet]
        [Authorize(Roles = Key.ROLE_ADMIN)]
        public ActionResult Delete(string id)
        {
            var model = _nRepo.CreateDeleteModel(id);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = Key.ROLE_ADMIN)]
        public ActionResult Delete(NotificationDetailVM model)
        {
            string result = "";
            if (ModelState.IsValid)
            {
                bool success = _nRepo.DeleteNotification(model.ReferenceID, out result);
                if (success)
                {
                    TempData["SuccessMsg"] = result;
                    // TODO handle when thread no longer exists
                    return RedirectToAction("DetailsThread", new { id = model.IncidentNumber });
                }
            }
            else
            {
                TempData["ErrorMsg"] = "Cannot delete Notification, try again later.";
            }
            TempData["ErrorMsg"] = result;
            model = _nRepo.CreateDeleteModel(model.ReferenceID);
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = Key.ROLE_ADMIN)]
        public ActionResult DeleteThread(string id)
        {
            var model = _nRepo.CreateDetailModel(id);

            // if notification returns null, redirect to notification index
            if (model==null)
            {
                TempData["ErrorMsg"] = "Cannot delete this thread at the moment";
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = Key.ROLE_ADMIN)]
        public ActionResult DeleteThread(ThreadDetailVM model)
        {
            string result = "";
            if (ModelState.IsValid)
            {
                bool success = _nRepo.DeleteThread(model.IncidentNumber, out result);
                if (success)
                {
                    TempData["SuccessMsg"] = result;
                    return RedirectToAction("Index");
                }
            }
            else
            {
                ViewBag.ErrorMsg = "Cannot add Notification, model not valid.";
            }

            // recreate the model for the view
            model = _nRepo.CreateDetailModel(model.IncidentNumber);
            // if notification returns null, redirect to notification index
            if (model == null)
            {
                TempData["ErrorMsg"] = "Cannot delete this thread at the moment";
                return RedirectToAction("Index");
            }
            TempData["ErrorMsg"] = result;
            return View(model);
        }
    }
}