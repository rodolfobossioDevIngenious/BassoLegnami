using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BassoLegnami.Model.Models.Support
{
	public class Festivity : In.Core.Models.Auditable
	{
		[Display(ResourceType = typeof(Resources.Models.Support.Festivity.Festivity), Name = "ObjectName", Description = "ObjectDescription")]
		public int FestivityID { get; set; }

		[Display(ResourceType = typeof(Resources.Models.Support.Festivity.Festivity), Name = nameof(Name))]
		[StringLength(100, ErrorMessageResourceName = nameof(SharedResource.MaxLength), ErrorMessageResourceType = typeof(SharedResource))]
		[Required(ErrorMessageResourceName = nameof(SharedResource.FieldRequired), ErrorMessageResourceType = typeof(SharedResource))]
		public string Name { get; set; }

		[Display(ResourceType = typeof(Resources.Models.Support.Festivity.Festivity), Name = nameof(Day))]
		[Range(1, 31, ErrorMessageResourceName = nameof(SharedResource.InvalidValue), ErrorMessageResourceType = typeof(SharedResource))]
		public int Day { get; set; }

		[Display(ResourceType = typeof(Resources.Models.Support.Festivity.Festivity), Name = nameof(Month))]
		[Range(1, 12, ErrorMessageResourceName = nameof(SharedResource.InvalidValue), ErrorMessageResourceType = typeof(SharedResource))]
		public int Month { get; set; }

		[Display(ResourceType = typeof(Resources.Models.Support.Festivity.Festivity), Name = nameof(Year))]
		public int? Year { get; set; }

		[Display(ResourceType = typeof(Resources.Models.Support.Festivity.Festivity), Name = nameof(City))]
		[StringLength(100, ErrorMessageResourceName = nameof(SharedResource.MaxLength), ErrorMessageResourceType = typeof(SharedResource))]
		public string City { get; set; }

		public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			List<ValidationResult> output = new();

			int year = DateTime.Now.Year;
			if (Year.HasValue)
			{
				year = Year.Value;
			}

			try
			{
				DateTime test = new(year, Month, Day);
			}
			catch (Exception)
			{
				output.Add(new ValidationResult(SharedResource.InvalidValue, new string[] { nameof(Day), nameof(Month), nameof(Year) }));
			}

			return output;
		}
	}
}
