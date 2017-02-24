using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Golf_3_Web_Forms.Startup))]
namespace Golf_3_Web_Forms
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
