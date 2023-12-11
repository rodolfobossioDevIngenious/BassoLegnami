using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace In.Core.Models
{
	// Add profile data for application users by adding properties to the ApplicationUser class
	public class ApplicationUser : IdentityUser
	{
		public const string USER_ADMINISTRATOR = "Administrator";
		public const string USER_SYSTEM = "System";

		public bool Enabled { get; set; }
		public bool ChangePassword { get; set; }
		public DateTime? LastPasswordChangedDate { get; set; }
	}
}
