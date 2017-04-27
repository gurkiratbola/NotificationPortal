using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NotificationPortal.ViewModels
{
    public class ValidationVM
    {
        public class ApplicationVM
        {
            [Required]
            public int ApplicationID { get; set; }

            [Required]
            public string ApplicationName { get; set; }

            [Required]
            public string Description { get; set; }

            [Required]
            public string URL { get; set; }

            [Required]
            public int StatusID { get; set; }

            public int ClientID { get; set; }
        }

        public class ClientVM
        {
            [Required]
            public int ClientID { get; set; }

            [Required]
            public string ClientName { get; set; }

            [Required]
            public int StatusID { get; set; }

            [Required]
            public string StatusName { get; set; }
        }

        public class NotificationVM
        {
            [Required]
            public int NotificationID { get; set; }

            [Required]
            public int ThreadID { get; set; }

            [Required]
            public string ReferenceID { get; set; }

            [Required]
            public string NotificaionHeading { get; set; }

            [Required]
            public string NotificaionDescription { get; set; }

            [Required]
            public DateTime SentDateTime { get; set; }

            [Required]
            public DateTime StartDateTime { get; set; }

            [Required]
            public DateTime EndDateTime { get; set; }

            [Required]
            public int? ApplicationID { get; set; }

            [Required]
            public int LevelOfImpactID { get; set; }

            [Required]
            public int NotificationTypeID { get; set; }

            [Required]
            public int SentMethodID { get; set; }

            [Required]
            public int StatusID { get; set; }

            [Required]
            public int ServerID { get; set; }
        }

        public class ServerVM
        {
            [Required]
            public int ServerID { get; set; }

            [Required]
            public int ServerStatusID { get; set; }

            [Required]
            public string ServerName { get; set; }
        }

        public class UserDetailVM
        {
            [Required]
            public int UserID { get; set; }

            [Required]
            public string FirstName { get; set; }

            [Required]
            public string LastName { get; set; }

            [Required]
            public string BusinessTitle { get; set; }

            [Required]
            public string BusinessPhone { get; set; }

            [Required]
            public string MobilePhone { get; set; }

            [Required]
            public string HomePhone { get; set; }

            [Required]
            public int ClientID { get; set; }

            [Required]
            public int StatusID { get; set; }
        }
    }
}