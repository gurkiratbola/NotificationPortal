using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotificationPortal.Models
{
    public static class Key
    {
        public const string DATA_CENTER_LOCATION_TORONTO = "Toronto";
        public const string DATA_CENTER_LOCATION_VANCOUVER = "Vancouver";

        public const string GROUP_INTERNAL = "Internal";
        public const string GROUP_EXTERNAL = "External";

        public const string LEVEL_OF_IMPACT_IMPACTING = "Impacting";
        public const string LEVEL_OF_IMPACT_NON_IMPACTING = "Non-Impacting";
        public const string LEVEL_OF_IMPACT_OUTAGE = "Full service outage";
        public const string LEVEL_OF_IMPACT_REDUNDANCY = "Loss of redundancy";

        public const string NOTIFICATION_TYPE_INCIDENT = "Incident";
        public const string NOTIFICATION_TYPE_MAINTENANCE = "Maintenance";

        public const string ROLE_ADMIN = "Admin";
        public const string ROLE_STAFF = "Staff";
        public const string ROLE_CLIENT = "Client";
        public const string ROLE_USER = "User";
        
        public const string SEND_METHOD_EMAIL = "Email";
        public const string SEND_METHOD_SMS = "SMS";

        public const string SERVER_TYPE_APPLICATION = "Application";
        public const string SERVER_TYPE_DATABASE = "Database";
        public const string SERVER_TYPE_DIRECTORY = "Directory";

        public const string STATUS_TYPE_APPLICATION = "Application";
        public const string STATUS_TYPE_CLIENT = "Client";
        public const string STATUS_TYPE_NOTIFICATION = "Notification";
        public const string STATUS_TYPE_SERVER = "Server";
        public const string STATUS_TYPE_USER = "User";

        public const string STATUS_APPLICATION_OFFLINE = "Offline";
        public const string STATUS_APPLICATION_ONLINE = "Online";
        public const string STATUS_CLIENT_DISABLED = "Disabled";
        public const string STATUS_CLIENT_ENABLED = "Enabled";
        public const string STATUS_NOTIFICATION_INCOMPLETE = "Incomplete";
        public const string STATUS_NOTIFICATION_COMPLETE = "Complete";
        public const string STATUS_SERVER_OFFLINE = "Offline";
        public const string STATUS_SERVER_ONLINE = "Online";
        public const string STATUS_USER_DISABLED = "Disabled";
        public const string STATUS_USER_ENABLED = "Enabled";
    }
}