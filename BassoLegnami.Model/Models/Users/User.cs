using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BassoLegnami.Model.Models.Users
{
	public class User
	{
		[Display(Name = "ObjectName", Description = "ObjectDescription")]
		public string UserId { get; set; }

		[Required(ErrorMessageResourceName = nameof(SharedResource.FieldRequired), ErrorMessageResourceType = typeof(SharedResource))]
		[Display(Name = nameof(UserName))]
		public string UserName { get; set; }

		[Required(ErrorMessageResourceName = nameof(SharedResource.FieldRequired), ErrorMessageResourceType = typeof(SharedResource))]
		[EmailAddress]
		[Display(Name = nameof(Email))]
		public string Email { get; set; }

		[Required(ErrorMessageResourceName = nameof(SharedResource.FieldRequired), ErrorMessageResourceType = typeof(SharedResource))]
		[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = nameof(Password))]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = nameof(ConfirmPassword))]
		[Compare(nameof(Password))]
		public string ConfirmPassword { get; set; }

		[Display(Name = nameof(Enabled))]
		public bool Enabled { get; set; }

		[Display(Name = nameof(ChangePassword))]
		public bool ChangePassword { get; set; }

		public ICollection<Role> Roles { get; set; }

		public User()
		{
			Roles = new HashSet<Role>();
		}
	}

	public class Role
	{
		[Display(Name = "ObjectName", Description = "ObjectDescription")]
		public string RoleId { get; set; }

		[Display(Name = nameof(Name))]
		public string Name { get; set; }
	}
}
