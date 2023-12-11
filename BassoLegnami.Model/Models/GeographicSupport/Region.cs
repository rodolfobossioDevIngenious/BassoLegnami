using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace BassoLegnami.Model.Models.GeographicSupport
{
	public partial class Region : In.Core.Models.Auditable
	{
		[Display(Name = "ObjectName", Description = "ObjectDescription")]
		public int RegionID { get; set; }

		[Required(ErrorMessageResourceType = typeof(Resources.Models.SharedResource), ErrorMessageResourceName = "FieldRequired")]
		[Display(ResourceType = typeof(Resources.Models.GeographicSupport.Region.Region), Name = "Name")]
		[StringLength(100)]
		public string Name { get; set; }

		[Required(ErrorMessageResourceType = typeof(Resources.Models.SharedResource), ErrorMessageResourceName = "FieldRequired")]
		[Display(ResourceType = typeof(Resources.Models.GeographicSupport.Region.Region), Name = "ISTATCode")]
		[StringLength(2, MinimumLength = 2)]
		public string ISTATCode { get; set; }

		[Required(ErrorMessageResourceType = typeof(Resources.Models.SharedResource), ErrorMessageResourceName = "FieldRequired")]
		[Display(ResourceType = typeof(Resources.Models.GeographicSupport.Region.Region), Name = "RegionalZoneID")]
		public int RegionalZoneID { get; set; }

		[Display(ResourceType = typeof(Resources.Models.GeographicSupport.Region.Region), Name = "Flag1")]
		[StringLength(3)]
		public string Flag1 { get; set; }

		public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			return new List<ValidationResult>();
		}

		public virtual RegionalZone RegionalZone { get; set; }
		public virtual ICollection<Province> Provinces { get; set; }

		public Region()
		{
		}
	}
}
