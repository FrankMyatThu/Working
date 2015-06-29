using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DBFirstHelper.Startup))]
namespace DBFirstHelper
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
