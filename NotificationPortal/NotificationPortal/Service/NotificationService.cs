using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using Twilio;
using Twilio.Types;
using Twilio.Rest.Api.V2010.Account;
using NotificationPortal.ViewModels;
using System.IO;

namespace NotificationPortal.Service
{
    public static class NotificationService
    {
        public static Task SendEmail(MailMessage mail)
        {
            // Pull Smtp config information from web.config
            string smtpHost = System.Configuration.ConfigurationManager.AppSettings["SmtpHost"];
            string smtpEmail = System.Configuration.ConfigurationManager.AppSettings["SmtpEmail"];
            string smtpPassword = System.Configuration.ConfigurationManager.AppSettings["SmtpPassword"];

            // send the message 
            SmtpClient smtp = new SmtpClient(smtpHost);
            NetworkCredential Credentials = new NetworkCredential(smtpEmail, smtpPassword);
            smtp.Credentials = Credentials;
            return smtp.SendMailAsync(mail);
        }

        public static async Task SendEmail(List<MailMessage> mails)
        {
            foreach (MailMessage mail in mails)
            {
                await SendEmail(mail);
            }
        }

        public static async Task<MessageResource> SendSMS(PhoneNumber phoneNumber, string bodyText)
        {
            string accountSid = System.Configuration.ConfigurationManager.AppSettings["TwilioAccountSID"];
            string authToken = System.Configuration.ConfigurationManager.AppSettings["TwilioAuthToken"];
            string fromNumber = System.Configuration.ConfigurationManager.AppSettings["TwilioFromNumber"];

            TwilioClient.Init(accountSid, authToken);
            
            var message = MessageResource.CreateAsync(
                phoneNumber,
                from: new PhoneNumber(fromNumber),
                body: bodyText);

            //Twilio doesn't currently have an async API, so return success.
            return await message;
        }

        public static async Task SendSMS(List<PhoneNumber> phoneNumbers, string bodyText)
        {
            foreach (PhoneNumber phoneNumber in phoneNumbers)
            {
                await SendSMS(phoneNumber, bodyText);
            }
        }

        public static string EmailTemplate(NotificationCreateVM model)
        {
            string path = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Service/NotificationEmailTemplate.html"));

            path.Replace("{Subject}", model.NotificationHeading);
            path.Replace("{Description}", model.NotificationDescription);
            path.Replace("{IncidentNumber}", model.IncidentNumber);
            path.Replace("{StartTime}", model.StartDateTime.ToString());
            path.Replace("{EndTime}", model.EndDateTime.ToString());

            return path;
        }
    }
}