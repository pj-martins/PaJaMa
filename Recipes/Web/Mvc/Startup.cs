using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PaJaMa.Recipes.Web.Mvc.Startup))]
namespace PaJaMa.Recipes.Web.Mvc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
