using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace BassoLegnami.Model.Models.GeographicSupport
{
	public class City : In.Core.Models.Auditable
	{
		[Display(Name = "ObjectName", Description = "ObjectDescription")]
		public int CityID { get; set; }

		[Required(ErrorMessageResourceType = typeof(Resources.Models.SharedResource), ErrorMessageResourceName = "FieldRequired")]
		[Display(ResourceType = typeof(Resources.Models.GeographicSupport.City.City), Name = "ProvinceID")]
		public int ProvinceID { get; set; }

		[Required(ErrorMessageResourceType = typeof(Resources.Models.SharedResource), ErrorMessageResourceName = "FieldRequired")]
		[Display(ResourceType = typeof(Resources.Models.GeographicSupport.City.City), Name = "Name")]
		[StringLength(100)]
		public string Name { get; set; }

		[Required(ErrorMessageResourceType = typeof(Resources.Models.SharedResource), ErrorMessageResourceName = "FieldRequired")]
		[Display(ResourceType = typeof(Resources.Models.GeographicSupport.City.City), Name = "CAP")]
		[StringLength(5)]
		[RegularExpression("^[0-9]{5}?", ErrorMessageResourceType = typeof(Resources.Models.GeographicSupport.City.City), ErrorMessageResourceName = "CAPError")]
		public string CAP { get; set; }

		[Required(ErrorMessageResourceType = typeof(Resources.Models.SharedResource), ErrorMessageResourceName = "FieldRequired")]
		[Display(ResourceType = typeof(Resources.Models.GeographicSupport.City.City), Name = "ISTATCode")]
		[StringLength(6, MinimumLength = 6, ErrorMessageResourceType = typeof(Resources.Models.GeographicSupport.City.City), ErrorMessageResourceName = "ISTATCodeError")]
		public string ISTATCode { get; set; }

		[Required(ErrorMessageResourceType = typeof(Resources.Models.SharedResource), ErrorMessageResourceName = "FieldRequired")]
		[Display(ResourceType = typeof(Resources.Models.GeographicSupport.City.City), Name = "RegisterCode")]
		[StringLength(4, MinimumLength = 4, ErrorMessageResourceType = typeof(Resources.Models.GeographicSupport.City.City), ErrorMessageResourceName = "RegisterCodeError")]
		public string RegisterCode { get; set; }

		[Display(ResourceType = typeof(Resources.Models.GeographicSupport.City.City), Name = "Flag1")]
		[StringLength(3)]
		public string Flag1 { get; set; }
		public double? Latitude { get; set; }
		public double? Longitude { get; set; }

		public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			return new List<ValidationResult>();
		}

		public virtual Province Province { get; set; }
	}
}
