using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(notejamsinglepage.Startup))]
namespace notejamsinglepage
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
