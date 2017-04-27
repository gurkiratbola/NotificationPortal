using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NotificationPortal.Startup))]
namespace NotificationPortal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
