using System.Web.Mvc;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(iNetMonitor.Web.Startup))]
namespace iNetMonitor.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

#if !DEBUG
            // Add HTTPS Redirect only on Release.
            GlobalFilters.Filters.Add(new RequireHttpsAttribute());
#endif
        }
    }
}
