using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

[assembly: OwinStartup(typeof(PaJaMa.Recipes.Web.Api.Startup))]
namespace PaJaMa.Recipes.Web.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            IdentityConfig.ConfigureAuth(app);
        }
    }
}