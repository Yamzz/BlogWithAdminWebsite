using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Guffaw.Startup))]
namespace Guffaw
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
