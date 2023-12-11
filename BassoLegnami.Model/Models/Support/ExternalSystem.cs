using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BassoLegnami.Model.Models.Support
{
	public class ExternalSystem : In.Core.Models.Auditable
	{
		[Display(Name = "ObjectName", Description = "ObjectDescription")]
		public int ExternalSystemID { get; set; }

		[Display(Name = nameof(Name))]
		[StringLength(100, ErrorMessageResourceName = nameof(SharedResource.MaxLength), ErrorMessageResourceType = typeof(SharedResource))]
		[Required(ErrorMessageResourceName = nameof(SharedResource.FieldRequired), ErrorMessageResourceType = typeof(SharedResource))]
		public string Name { get; set; }

		[Display(Name = nameof(Key))]
		[StringLength(100, ErrorMessageResourceName = nameof(SharedResource.MaxLength), ErrorMessageResourceType = typeof(SharedResource))]
		[Required(ErrorMessageResourceName = nameof(SharedResource.FieldRequired), ErrorMessageResourceType = typeof(SharedResource))]
		public string Key { get; set; }

		public virtual ICollection<ExternalSystemValue> ExternalSystemValues { get; set; }

		public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			return Enumerable.Empty<ValidationResult>();
		}

		public ExternalSystem()
		{
			ExternalSystemValues = new HashSet<ExternalSystemValue>();
		}
	}
}