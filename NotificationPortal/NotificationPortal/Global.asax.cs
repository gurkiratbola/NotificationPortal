using NotificationPortal.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Http;
using System.Web.Routing;

namespace NotificationPortal
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        internal protected void Application_BeginRequest(object sender, EventArgs e)
        {
            // encrypt sensitive info
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                EncryptionHelper encryptionHelper = new EncryptionHelper();
                encryptionHelper.EncryptStrings("connectionStrings");
                encryptionHelper.EncryptStrings("appSettings");
            }
        }
    }
}
