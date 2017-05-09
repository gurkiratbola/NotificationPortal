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
        public const string LEVEL_OF_IMPACT_OUTAGE = "Full service outage";
        public const string LEVEL_OF_IMPACT_REDUNDANCY = "Loss of redundancy";
        public const string LEVEL_OF_IMPACT_NON_IMPACTING = "Non-Impacting";

        public const int LEVEL_OF_IMPACT_IMPACTING_VALUE = 4;
        public const int LEVEL_OF_IMPACT_OUTAGE_VALUE = 3;
        public const int LEVEL_OF_IMPACT_REDUNDANCY_VALUE = 2;
        public const int LEVEL_OF_IMPACT_NON_IMPACTING_VALUE = 1;

        public const string NOTIFICATION_TYPE_INCIDENT = "Incident";
        public const string NOTIFICATION_TYPE_MAINTENANCE = "Maintenance";

        public const string PRIORITY_NAME_HIGH = "High";
        public const string PRIORITY_NAME_NORMAL = "Normal";
        public const string PRIORITY_NAME_LOW = "Low";

        public const int PRIORITY_VALUE_HIGH = 3;
        public const int PRIORITY_VALUE_NORMAL = 2;
        public const int PRIORITY_VALUE_LOW = 1;

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
        public const string STATUS_NOTIFICATION_OPEN = "Open";
        public const string STATUS_NOTIFICATION_CLOSED = "Closed";
        public const string STATUS_SERVER_OFFLINE = "Offline";
        public const string STATUS_SERVER_ONLINE = "Online";
        public const string STATUS_USER_DISABLED = "Disabled";
        public const string STATUS_USER_ENABLED = "Enabled";
    }
}