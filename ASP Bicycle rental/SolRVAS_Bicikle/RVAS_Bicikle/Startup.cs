using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RVAS_Bicikle.Startup))]
namespace RVAS_Bicikle
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
