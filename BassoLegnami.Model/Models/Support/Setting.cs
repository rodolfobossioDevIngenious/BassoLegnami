using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BassoLegnami.Model.Models.Support
{
	public class Setting : In.Core.Models.Auditable
	{
		[Display(Name = "ObjectName", Description = "ObjectDescription")]
		public int SettingID { get; set; }

		[Display(Name = nameof(Name))]
		[StringLength(100, ErrorMessageResourceName = nameof(SharedResource.MaxLength), ErrorMessageResourceType = typeof(SharedResource))]
		[Required(ErrorMessageResourceName = nameof(SharedResource.FieldRequired), ErrorMessageResourceType = typeof(SharedResource))]
		public string Name { get; set; }

		[Display(Name = nameof(Key))]
		[StringLength(100, ErrorMessageResourceName = nameof(SharedResource.MaxLength), ErrorMessageResourceType = typeof(SharedResource))]
		[Required(ErrorMessageResourceName = nameof(SharedResource.FieldRequired), ErrorMessageResourceType = typeof(SharedResource))]
		public string Key { get; set; }

		[Display(Name = nameof(Value))]
		[StringLength(200, ErrorMessageResourceName = nameof(SharedResource.MaxLength), ErrorMessageResourceType = typeof(SharedResource))]
		[Required(ErrorMessageResourceName = nameof(SharedResource.FieldRequired), ErrorMessageResourceType = typeof(SharedResource))]
		public string Value { get; set; }

		[StringLength(5000, ErrorMessageResourceName = nameof(SharedResource.MaxLength), ErrorMessageResourceType = typeof(SharedResource))]
		[Display(Name = nameof(Note), ResourceType = typeof(SharedResource))]
		[DataType(DataType.MultilineText)]
		public string Note { get; set; }

		public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			return Enumerable.Empty<ValidationResult>();
		}
	}
}
