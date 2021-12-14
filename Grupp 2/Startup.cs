using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Grupp_2.Startup))]
namespace Grupp_2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
