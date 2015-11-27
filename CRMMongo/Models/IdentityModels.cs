using System;
using System.Collections.Generic;
using System.Configuration;
using MongoDB.AspNet.Identity;
using System.Security.Claims;
using System.Threading.Tasks;
using CRMMongo.Properties;
using Microsoft.AspNet.Identity;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;


namespace CRMMongo.Models {
	// You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
	public class ApplicationUser : IdentityUser {
		public string Hometown { get; set; }
		public string Email { get; set; }
		public string PhoneNumber { get; set; }
		public bool EmailConfirmed{ get; set; }
		public bool PhoneNumberConfirmed { get; set; }
		public bool TwoFactorEnabled { get; set; }
		public DateTimeOffset LockoutEndDateUtc { get; set; }
		public int AccessFailedCount { get; set; }
		public bool LockoutEnabled { get; set; }

		public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager) {
			// Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
			var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
			// Add custom user claims here
			return userIdentity;
		}
	}

	public class UserStore : UserStore<ApplicationUser>, IUserEmailStore<ApplicationUser>, IUserPhoneNumberStore<ApplicationUser>, IUserTwoFactorStore<ApplicationUser, string>,
		IUserLockoutStore<ApplicationUser, string> {
		private MongoDatabase db;
		public UserStore() : base(Settings.Default.mongoDbConnectionString) {
			setDB(Settings.Default.mongoDbConnectionString);
		}

		public virtual Task<bool> GetEmailConfirmedAsync(ApplicationUser user) {
			return Task.FromResult(user.EmailConfirmed);
		}

		public virtual Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed) {
			user.EmailConfirmed = confirmed;
			return Task.FromResult(0);
		}

		public virtual Task SetEmailAsync(ApplicationUser user, string email) {
			user.Email = email;
			return Task.FromResult(0);
		}

		public virtual Task<string> GetEmailAsync(ApplicationUser user) {
			return Task.FromResult(user.Email);
		}

		public virtual Task<ApplicationUser> FindByEmailAsync(string email) {
			return Task.FromResult<ApplicationUser>(db.GetCollection<ApplicationUser>("AspNeApplicationUsers").FindOne(Query.EQ("email", (BsonValue)email)));
		}

		public virtual Task SetPhoneNumberAsync(ApplicationUser user, string phoneNumber) {
			user.PhoneNumber = phoneNumber;
			return Task.FromResult(0);
		}

		public virtual Task<string> GetPhoneNumberAsync(ApplicationUser user) {
			return Task.FromResult(user.PhoneNumber);
		}

		public virtual Task<bool> GetPhoneNumberConfirmedAsync(ApplicationUser user) {
			return Task.FromResult(user.PhoneNumberConfirmed);
		}

		public virtual Task SetPhoneNumberConfirmedAsync(ApplicationUser user, bool confirmed) {
			user.PhoneNumberConfirmed = confirmed;
			return Task.FromResult(0);
		}
		private void setDB(string connectionUrl) {
			if (connectionUrl.ToLower().StartsWith("mongodb://")) {
				db = this.GetDatabaseFromUrl(new MongoUrl(connectionUrl));
			}
			else {
				string connectionString = ConfigurationManager.ConnectionStrings[connectionUrl].ConnectionString;
				if (connectionString.ToLower().StartsWith("mongodb://"))
					db = this.GetDatabaseFromUrl(new MongoUrl(connectionString));
			}
		}

		private MongoDatabase GetDatabaseFromUrl(MongoUrl url) {
			MongoServer server = new MongoClient(url).GetServer();
			if (url.DatabaseName == null)
				throw new Exception("No database name specified in connection string");
			return server.GetDatabase(url.DatabaseName);
		}

		public virtual Task SetTwoFactorEnabledAsync(ApplicationUser user, bool enabled) {
			user.TwoFactorEnabled = enabled;
			return Task.FromResult(0);
		}

		public virtual Task<bool> GetTwoFactorEnabledAsync(ApplicationUser user) {
			return Task.FromResult(user.TwoFactorEnabled);
		}

		public virtual Task<DateTimeOffset> GetLockoutEndDateAsync(ApplicationUser user) {
			return Task.FromResult(user.LockoutEndDateUtc);
		}

		public virtual Task SetLockoutEndDateAsync(ApplicationUser user, DateTimeOffset lockoutEnd) {
			user.LockoutEndDateUtc = new DateTime(lockoutEnd.Ticks, DateTimeKind.Utc);
			return Task.FromResult(0);
		}

		public virtual Task<int> IncrementAccessFailedCountAsync(ApplicationUser user) {
			user.AccessFailedCount++;
			return Task.FromResult(user.AccessFailedCount);
		}

		public virtual Task ResetAccessFailedCountAsync(ApplicationUser user) {
			user.AccessFailedCount = 0;
			return Task.FromResult(0);
		}

		public virtual Task<int> GetAccessFailedCountAsync(ApplicationUser user) {
			return Task.FromResult(user.AccessFailedCount);
		}

		public virtual Task<bool> GetLockoutEnabledAsync(ApplicationUser user) {
			return Task.FromResult(user.LockoutEnabled);
		}

		public virtual Task SetLockoutEnabledAsync(ApplicationUser user, bool enabled) {
			user.LockoutEnabled = enabled;
			return Task.FromResult(0);
		}
	}

}