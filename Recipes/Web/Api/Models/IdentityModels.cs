using Microsoft.AspNet.Identity;
using PaJaMa.Recipes.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace PaJaMa.Recipes.Web.Api.Models
{
	public class ApplicationUser : IUser
	{
		public User User { get; set; }

		public string Id
		{
			get { return User.UserID.ToString(); }
		}


		public string UserName
		{
			get { return User.UserName; }
			set { User.UserName = value; }
		}
		
		public ApplicationUser(User user)
		{
			User = user;
		}
		
		public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
		{
			// Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
			var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
			// Add custom user claims here
			return userIdentity;
		}
	}

	public class UserStore : IUserStore<ApplicationUser>
	{
		public Task CreateAsync(ApplicationUser user)
		{
			throw new NotImplementedException();
		}

		public Task DeleteAsync(ApplicationUser user)
		{
			throw new NotImplementedException();
		}

		public Task<ApplicationUser> FindByIdAsync(string userId)
		{
			throw new NotImplementedException();
		}

		public Task<ApplicationUser> FindByNameAsync(string userName)
		{
			throw new NotImplementedException();
		}

		public Task UpdateAsync(ApplicationUser user)
		{
			throw new NotImplementedException();
		}

		public void Dispose()
		{
		}
	}
}