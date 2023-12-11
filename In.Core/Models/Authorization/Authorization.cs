using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace In.Core.Models.Authorization
{
	public class Authorization : Auditable
	{
		public const string ROLE_ADMINISTRATORS = "Administrators";

		[Display(Name = "ObjectName", Description = "ObjectDescription")]
		public int AuthorizationID { get; set; }

		[Display(Name = nameof(RoleId))]
		[StringLength(450)]
		[Required()]
		public string RoleId { get; set; }

		[Display(Name = nameof(Controller))]
		[StringLength(100)]
		[Required()]
		public string Controller { get; set; }

		[Display(Name = nameof(Action))]
		[StringLength(100)]
		[Required()]
		public string Action { get; set; }

		[Display(Name = nameof(Authorized))]
		public bool Authorized { get; set; }

		[Display(Name = nameof(Note))]
		[StringLength(500)]
		[DataType(DataType.MultilineText)]
		public string Note { get; set; }

		public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			return Enumerable.Empty<ValidationResult>();
		}
	}
}
