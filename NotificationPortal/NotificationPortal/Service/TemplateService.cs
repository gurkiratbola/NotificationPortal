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
    }
}