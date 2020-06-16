using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NewsWeb.Startup))]
namespace NewsWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
