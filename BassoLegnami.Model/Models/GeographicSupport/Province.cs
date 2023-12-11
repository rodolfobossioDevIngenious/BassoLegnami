using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace BassoLegnami.Model.Models.GeographicSupport
{
	public class Province : In.Core.Models.Auditable
	{
		[Display(Name = "ObjectName", Description = "ObjectDescription")]
		public int ProvinceID { get; set; }

		[Required(ErrorMessageResourceType = typeof(Resources.Models.SharedResource), ErrorMessageResourceName = "FieldRequired")]
		[Display(ResourceType = typeof(Resources.Models.GeographicSupport.Province.Province), Name = "RegionID")]
		public int RegionID { get; set; }

		[Required(ErrorMessageResourceType = typeof(Resources.Models.SharedResource), ErrorMessageResourceName = "FieldRequired")]
		[Display(ResourceType = typeof(Resources.Models.GeographicSupport.Province.Province), Name = "Name")]
		[StringLength(50)]
		public string Name { get; set; }

		[Required(ErrorMessageResourceType = typeof(Resources.Models.SharedResource), ErrorMessageResourceName = "FieldRequired")]
		[Display(ResourceType = typeof(Resources.Models.GeographicSupport.Province.Province), Name = "Abbreviation")]
		[StringLength(2)]
		public string Abbreviation { get; set; }

		[Required(ErrorMessageResourceType = typeof(Resources.Models.SharedResource), ErrorMessageResourceName = "FieldRequired")]
		[Display(ResourceType = typeof(Resources.Models.GeographicSupport.Province.Province), Name = "ISTATCode")]
		[StringLength(3, MinimumLength = 3)]
		public string ISTATCode { get; set; }

		[Display(ResourceType = typeof(Resources.Models.GeographicSupport.Province.Province), Name = "Flag1")]
		[StringLength(3)]
		public string Flag1 { get; set; }

		public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			return new List<ValidationResult>();
		}

		public virtual Region Region { get; set; }
		public virtual ICollection<City> Cities { get; set; }

		public Province()
		{
			Cities = new HashSet<City>();
		}
	}
}
