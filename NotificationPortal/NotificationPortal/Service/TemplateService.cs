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
        public static string NotificationEmail(NotificationCreateVM model)
        {
            string path = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Service/templates/NotificationEmailTemplate.html"));

            path = path.Replace("{Subject}", model.NotificationHeading)
                   .Replace("{Description}", model.NotificationDescription)
                   .Replace("{IncidentNumber}", model.IncidentNumber)
                   .Replace("{StartTime}", model.StartDateTime == null ? DateTime.Now.ToString() : model.StartDateTime.ToString())
                   .Replace("{EndTime}", model.EndDateTime == null ? "To Be Announced" : model.EndDateTime.ToString());

            return path;
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
                   .Replace("{Url}", "http://" + HttpContext.Current.Request.Url.Authority + "/Notification/DetailsThread/" + model.IncidentNumber);

            return message;
        }
    }
}