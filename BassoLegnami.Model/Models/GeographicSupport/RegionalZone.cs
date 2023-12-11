using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace BassoLegnami.Model.Models.GeographicSupport
{
	public partial class RegionalZone : In.Core.Models.Auditable
	{
		[Display(Name = "ObjectName", Description = "ObjectDescription")]
		public int RegionalZoneID { get; set; }

		[Required(ErrorMessageResourceType = typeof(Resources.Models.SharedResource), ErrorMessageResourceName = "FieldRequired")]
		[Display(ResourceType = typeof(Resources.Models.GeographicSupport.RegionalZone.RegionalZone), Name = "Name")]
		[StringLength(100)]
		public string Name { get; set; }

		public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			return new List<ValidationResult>();
		}

		public virtual ICollection<Region> Regions { get; set; }
	}
}
