using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotificationPortal.Models
{
    public static class Key
    {
        public static string DATA_CENTER_LOCATION_TORONTO = "Toronto";
        public static string DATA_CENTER_LOCATION_VANCOUVER = "Vancouver";

        public static string GROUP_INTERNAL = "Internal";
        public static string GROUP_EXTERNAL = "External";

        public static string LEVEL_OF_IMPACT_IMPACTING = "Impacting";
        public static string LEVEL_OF_IMPACT_NON_IMPACTING = "Non-Impacting";
        public static string LEVEL_OF_IMPACT_OUTAGE = "Full service outage";
        public static string LEVEL_OF_IMPACT_REDUNDANCY = "Loss of redundancy";

        public static string NOTIFICATION_TYPE_INCIDENT = "Incident";
        public static string NOTIFICATION_TYPE_MAINTENANCE = "Maintenance";

        public static string ROLE_ADMIN = "Admin";
        public static string ROLE_STAFF = "Staff";
        public static string ROLE_CLIENT = "Client";
        public static string ROLE_USER = "User";
        
        public static string SEND_METHOD_EMAIL = "Email";
        public static string SEND_METHOD_SMS = "SMS";

        public static string SERVER_TYPE_APPLICATION = "Application";
        public static string SERVER_TYPE_DATABASE = "Database";
        public static string SERVER_TYPE_DIRECTORY = "Directory";

        public static string STATUS_TYPE_APPLICATION = "Application";
        public static string STATUS_TYPE_CLIENT = "Client";
        public static string STATUS_TYPE_NOTIFICATION = "Notification";
        public static string STATUS_TYPE_SERVER = "Server";
        public static string STATUS_TYPE_USER = "User";

        public static string STATUS_APPLICATION_OFFLINE = "Offline";
        public static string STATUS_APPLICATION_ONLINE = "Online";
        public static string STATUS_CLIENT_DISABLED = "Disabled";
        public static string STATUS_CLIENT_ENABLED = "Enabled";
        public static string STATUS_NOTIFICATION_INCOMPLETE = "Incomplete";
        public static string STATUS_NOTIFICATION_COMPLETE = "Complete";
        public static string STATUS_SERVER_OFFLINE = "Offline";
        public static string STATUS_SERVER_ONLINE = "Online";
        public static string STATUS_USER_DISABLED = "Disabled";
        public static string STATUS_USER_ENABLED = "Enabled";
    }
}