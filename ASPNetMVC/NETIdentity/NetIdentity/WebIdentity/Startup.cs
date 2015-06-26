using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebIdentity.Startup))]
namespace WebIdentity
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
