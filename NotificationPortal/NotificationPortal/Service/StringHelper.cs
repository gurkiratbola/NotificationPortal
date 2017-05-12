using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace NotificationPortal.Service
{
    public static class StringHelper
    {
        public static string TruncateWithEllipsis(string s)
        {
            const string Ellipsis = "&hellip;";
            const int LIMIT = 60;
            if (s.Length > LIMIT)
                return s.Substring(0, s.Length - Ellipsis.Length) + Ellipsis;
            else
                return s;
        }

        public static string RemoveTags(string text)
        {
            string s = Regex.Replace(text, @"<.*?>", string.Empty);
            s = s.Replace("&nbsp;", " ");
            return s;
        }
    }
}