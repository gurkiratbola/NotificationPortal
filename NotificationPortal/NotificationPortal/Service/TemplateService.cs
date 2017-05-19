using NotificationPortal.Models;
using NotificationPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace NotificationPortal.Service
{
    public static class TemplateService
    {
        /// define subdirectory here
        /// just "/" if using domain or subdomain
        /// "/directory_path/" if using subdirectory
        private const string SUB_DIRECTORY = "/";
        public static string AccountEmail(string callBackUrl, string body, string buttonText)
        {
            // load the html template and read the content to be replaced later.
            string path = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Service/templates/AccountEmailTemplate.html"));

            string message = path.Replace("{URL}", callBackUrl)
                             .Replace("{Body}", body)
                             .Replace("{ButtonText}", buttonText);

            return message;
        }

        public static string NotificationEmail(NotificationCreateVM model)
        {
            ApplicationDbContext _context = new ApplicationDbContext();

            // get application id and other information that is about to be sent
            var appsId = _context.Application.Where(a => model.ApplicationReferenceIDs.Contains(a.ReferenceID)).Select(app => app.ApplicationName);
            string levelOfImpact = _context.LevelOfImpact.FirstOrDefault(l => l.LevelOfImpactID == model.LevelOfImpactID).LevelName;
            string status = _context.Status.FirstOrDefault(s => s.StatusID == model.StatusID).StatusName;

            // loop through the list of applications if user has more than one application
            string apps = "";
            foreach (var app in appsId)
            {
                apps = app.ToString();
            }

            // calculate the duration the application / server were down for
            TimeSpan? duration = (model.EndDateTime - model.StartDateTime);

            // load the html template and read the content to be replaced later.
            string path = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Service/templates/NotificationEmailTemplate.html"));

            string message = "";
            message = path.Replace("{Subject}", model.NotificationHeading + ", " + apps)
                      .Replace("{Description}", model.NotificationDescription)
                      .Replace("{IncidentNumber}", model.IncidentNumber)
                      .Replace("{ServiceAffected}", apps)
                      .Replace("{ServiceImpact}", levelOfImpact)
                      .Replace("{CurrentState}", status)
                      .Replace("{StartTime}", model.StartDateTime == null ? DateTime.Now.ToString() : model.StartDateTime.ToString())
                      .Replace("{EndTime}", model.EndDateTime == null ? "Not available at this time" : model.EndDateTime.ToString())
                      .Replace("{Duration}", model.StartDateTime == null || model.EndDateTime == null ? "Not available at this time" : duration.ToString())
                      .Replace("{URL}", "http://" + HttpContext.Current.Request.Url.Authority + SUB_DIRECTORY +"Notification/DetailsThread/" + model.IncidentNumber);

            return message;
        }

        public static string NotificationSMS(NotificationCreateVM model)
        {
            ApplicationDbContext _context = new ApplicationDbContext();
            string levelOfImpact = _context.LevelOfImpact.FirstOrDefault(l => l.LevelOfImpactID == model.LevelOfImpactID).LevelName;
            string status = _context.Status.FirstOrDefault(s => s.StatusID == model.StatusID).StatusName;

            string template = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Service/templates/NotificationSMSTemplate.txt"));

            string message = template.Replace("{Subject}", model.NotificationHeading)
                   .Replace("{IncidentNumber}", model.IncidentNumber)
                   .Replace("{LevelOfImpact}", levelOfImpact)
                   .Replace("{Status}", status)
                   .Replace("{Url}", "http://" + HttpContext.Current.Request.Url.Authority + SUB_DIRECTORY + "Notification/DetailsThread/" + model.IncidentNumber);

            return message;
        }
    }
}