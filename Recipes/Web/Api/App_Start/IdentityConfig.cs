using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using PaJaMa.Recipes.Web.Api.Providers;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity;
using PaJaMa.Recipes.Model;
using Microsoft.AspNet.Identity.Owin;
using PaJaMa.Recipes.Web.Api.Models;

namespace PaJaMa.Recipes.Web.Api
{
	public class IdentityConfig
	{
		public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

		public static string PublicClientId { get; private set; }

		public static void ConfigureAuth(IAppBuilder app)
		{
			// Configure the db context and user manager to use a single instance per request
			//app.CreatePerOwinContext(ApplicationDbContext.Create);
			app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

			// Enable the application to use a cookie to store information for the signed in user
			// and to use a cookie to temporarily store information about a user logging in with a third party login provider
			app.UseCookieAuthentication(new CookieAuthenticationOptions());
			app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

			// Configure the application for OAuth based flow
			PublicClientId = "self";
			OAuthOptions = new OAuthAuthorizationServerOptions
			{
				TokenEndpointPath = new PathString("/Token"),
				Provider = new ApplicationOAuthProvider(PublicClientId),
				AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
				AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
				AllowInsecureHttp = true
			};

			// Enable the application to use bearer tokens to authenticate users
			app.UseOAuthBearerTokens(OAuthOptions);

			// Uncomment the following lines to enable logging in with third party login providers
			//app.UseMicrosoftAccountAuthentication(
			//    clientId: "",
			//    clientSecret: "");

			//app.UseTwitterAuthentication(
			//    consumerKey: "",
			//    consumerSecret: "");

			//app.UseFacebookAuthentication(
			//    appId: "",
			//    appSecret: "");

			//app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
			//{
			//    ClientId = "",
			//    ClientSecret = ""
			//});
		}
	}

	public class ApplicationUserManager : UserManager<ApplicationUser>
	{
		public ApplicationUserManager(IUserStore<ApplicationUser> store)
			: base(store)
		{
		}

		public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
		{
			var manager = new ApplicationUserManager(new UserStore());
			// Configure validation logic for usernames
			manager.UserValidator = new UserValidator<ApplicationUser>(manager)
			{
				AllowOnlyAlphanumericUserNames = false,
				RequireUniqueEmail = true
			};
			// Configure validation logic for passwords
			manager.PasswordValidator = new PasswordValidator
			{
				RequiredLength = 6,
				RequireNonLetterOrDigit = true,
				RequireDigit = true,
				RequireLowercase = true,
				RequireUppercase = true,
			};
			var dataProtectionProvider = options.DataProtectionProvider;
			if (dataProtectionProvider != null)
			{
				manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
			}
			return manager;
		}
	}
}