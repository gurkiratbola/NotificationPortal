using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace NotificationPortal.Service
{
    public static class NotificationService
    {
        public static Task SendEmail(MailMessage mail)
        {
            //send the message 
            SmtpClient smtp = new SmtpClient("mail.dakotajang.me");

            NetworkCredential Credentials = new NetworkCredential("admin_np@dakotajang.me", "P@ssw0rd!");
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
    }
}