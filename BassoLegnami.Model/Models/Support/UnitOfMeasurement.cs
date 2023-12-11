using BassoLegnami.Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BassoLegnami.Model.Models.Support
{
	public class UnitOfMeasurement : In.Core.Models.Auditable
	{
		[Display(ResourceType = typeof(Resources.Models.Support.UnitOfMeasurement.UnitOfMeasurement), Name = "ObjectName", Description = "ObjectDescription")]
		public int UnitOfMeasurementID { get; set; }

		[Required(ErrorMessageResourceType = typeof(Resources.Models.SharedResource), ErrorMessageResourceName = "FieldRequired")]
		[Display(ResourceType = typeof(Resources.Models.Support.UnitOfMeasurement.UnitOfMeasurement), Name = "Name")]
		[StringLength(250)]
		public string Name { get; set; }

		[Required(ErrorMessageResourceType = typeof(Resources.Models.SharedResource), ErrorMessageResourceName = "FieldRequired")]
		[Display(ResourceType = typeof(Resources.Models.Support.UnitOfMeasurement.UnitOfMeasurement), Name = "Symbol")]
		[StringLength(10)]
		public string Symbol { get; set; }

		[Display(ResourceType = typeof(Resources.Models.Support.UnitOfMeasurement.UnitOfMeasurement), Name = "IsAcceptDecimalVaule")]
		public bool IsAcceptDecimalVaule { get; set; }

		public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			return Enumerable.Empty<ValidationResult>();
		}

		public UnitOfMeasurement()
		{
		}
	}
}
