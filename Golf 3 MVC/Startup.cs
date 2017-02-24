using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Golf_3_MVC.Startup))]
namespace Golf_3_MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
