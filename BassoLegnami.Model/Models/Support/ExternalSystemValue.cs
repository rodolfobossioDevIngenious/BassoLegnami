using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BassoLegnami.Model.Models.Support
{
	public class ExternalSystemValue : In.Core.Models.Auditable
	{
		[Display(Name = "ObjectName", Description = "ObjectDescription")]
		public int ExternalSystemValueID { get; set; }

		[Display(Name = nameof(ExternalSystem))]
		[Required(ErrorMessageResourceName = nameof(SharedResource.FieldRequired), ErrorMessageResourceType = typeof(SharedResource))]
		public int ExternalSystemID { get; set; }

		[Display(Name = nameof(Value1))]
		[StringLength(100, ErrorMessageResourceName = nameof(SharedResource.MaxLength), ErrorMessageResourceType = typeof(SharedResource))]
		[Required(ErrorMessageResourceName = nameof(SharedResource.FieldRequired), ErrorMessageResourceType = typeof(SharedResource))]
		public string Value1 { get; set; }

		[Display(Name = nameof(Value2))]
		[StringLength(100, ErrorMessageResourceName = nameof(SharedResource.MaxLength), ErrorMessageResourceType = typeof(SharedResource))]
		public string Value2 { get; set; }

		[Display(Name = nameof(ExternalSystem))]
		public virtual ExternalSystem ExternalSystem { get; set; }

		public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			return Enumerable.Empty<ValidationResult>();
		}
	}
}
