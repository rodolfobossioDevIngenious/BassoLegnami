using In.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BassoLegnami.Model.Models.Log
{
	public class Error : Auditable
	{
		[Display(Name = "ObjectName", Description = "ObjectDescription")]
		public Guid ErrorID { get; set; }

		[Display(Name = nameof(Log))]
		public Guid LogID { get; set; }

		[Display(Name = nameof(Message))]
		[DataType(DataType.MultilineText)]
		public string Message { get; set; }

		[Display(Name = nameof(ExceptionName))]
		[DataType(DataType.MultilineText)]
		public string ExceptionName { get; set; }

		[Display(Name = nameof(StackTrace))]
		[DataType(DataType.MultilineText)]
		public string StackTrace { get; set; }

		[Display(Name = nameof(Source))]
		[DataType(DataType.MultilineText)]
		public string Source { get; set; }

		[Display(Name = nameof(Log))]
		public virtual Log Log { get; set; }

		public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			return Enumerable.Empty<ValidationResult>();
		}
	}
}
