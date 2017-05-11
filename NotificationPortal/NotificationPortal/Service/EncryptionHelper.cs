using System;
using System.Configuration;
using System.Web;
using System.Web.Configuration;

namespace NotificationPortal.Service
{
    // Encrypts the sensitive infomation in web.config file
    public class EncryptionHelper
    {
        // This gets called from Application_Start()
        public void EncryptStrings(string sectionTag)
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration(
                                           HttpContext.Current.Request.ApplicationPath);
            ConfigurationSection section = config.GetSection(sectionTag);
            if (section != null && !section.SectionInformation.IsProtected)
            {
                try
                {
                    section.SectionInformation.ProtectSection(
                            "DataProtectionConfigurationProvider");
                    config.Save();
                }
                catch (Exception ex)
                {
                    string errorMessage = ex.Message;
                }
            }
        }
    }
}