using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(UniversityMgtApp.Startup))]
namespace UniversityMgtApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
