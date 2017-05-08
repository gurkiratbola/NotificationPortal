using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotificationPortal.Repositories
{
    public static class ConstantsRepo
    {
        public const int PAGE_SIZE = 20;
        public const string SORT_CLIENT_BY_NAME_ASCE = "client_name_asce";
        public const string SORT_CLIENT_BY_NAME_DESC = "client_name_desc";

        public const string SORT_LEVEL_OF_IMPACT_ASCE = "level_of_impact_asce";
        public const string SORT_LEVEL_OF_IMPACT_DESC = "level_of_impact_desc";

        public const string SORT_NOTIFICATION_BY_HEADING_ASCE = "notification_heading_asce";
        public const string SORT_NOTIFICATION_BY_HEADING_DESC = "notification_heading_desc";

        public const string SORT_NOTIFICATION_BY_TYPE_ASCE = "notification_type_asce";
        public const string SORT_NOTIFICATION_BY_TYPE_DESC = "notification_type_desc";

        public const string SORT_STATUS_BY_NAME_ASCE = "status_name_asce";
        public const string SORT_STATUS_BY_NAME_DESC = "status_name_desc";

        public const string SORT_FIRST_NAME_BY_ASCE = "first_name_asce";
        public const string SORT_FIRST_NAME_BY_DESC = "first_name_desc";

        public const string SORT_APP_BY_NAME_ASCE = "application_name_asce";
        public const string SORT_APP_BY_NAME_DESC = "application_name_desc";

    }
}