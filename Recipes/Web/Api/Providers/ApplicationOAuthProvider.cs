using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

using PaJaMa.Common;
using PaJaMa.Recipes.Model;
using PaJaMa.Recipes.Web.Api.Models;
using PaJaMa.Recipes.Model.Entities;

namespace PaJaMa.Recipes.Web.Api.Providers
{
	public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
	{
		private readonly string _publicClientId;

		public ApplicationOAuthProvider(string publicClientId)
		{
			if (publicClientId == null)
			{
				throw new ArgumentNullException("publicClientId");
			}

			_publicClientId = publicClientId;
		}

		public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
		{
			var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();

			var db = new RecipesContext();

            string encrypted = EncrypterDecrypter.Instance.Encrypt(context.Password, User.ENCRYPT_PASSWORD);

			var user = db.Users.FirstOrDefault(u => u.UserName == context.UserName && u.Password ==
				encrypted);

			if (user == null)
			{
				context.SetError("invalid_grant", "The user name or password is incorrect.");
				return;
			}

			ApplicationUser appUser = new ApplicationUser(user);
			ClaimsIdentity oAuthIdentity = await appUser.GenerateUserIdentityAsync(userManager,
			   OAuthDefaults.AuthenticationType);
			ClaimsIdentity cookiesIdentity = await appUser.GenerateUserIdentityAsync(userManager,
				CookieAuthenticationDefaults.AuthenticationType);

			AuthenticationProperties properties = CreateProperties(user.UserName);
			AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
			context.Validated(ticket);
			context.Request.Context.Authentication.SignIn(cookiesIdentity);
		}

		public override Task TokenEndpoint(OAuthTokenEndpointContext context)
		{
			foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
			{
				context.AdditionalResponseParameters.Add(property.Key, property.Value);
			}

			return Task.FromResult<object>(null);
		}

		public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
		{
			// Resource owner password credentials does not provide a client ID.
			if (context.ClientId == null)
			{
				context.Validated();
			}

			return Task.FromResult<object>(null);
		}

		public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
		{
			if (context.ClientId == _publicClientId)
			{
				Uri expectedRootUri = new Uri(context.Request.Uri, "/");

				if (expectedRootUri.AbsoluteUri == context.RedirectUri)
				{
					context.Validated();
				}
			}

			return Task.FromResult<object>(null);
		}

		public static AuthenticationProperties CreateProperties(string userName)
		{
			IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName }
            };
			return new AuthenticationProperties(data);
		}
	}
}