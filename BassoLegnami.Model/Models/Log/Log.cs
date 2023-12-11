using In.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BassoLegnami.Model.Models.Log
{
	public class Log : Auditable
	{
		[Display(Name = "ObjectName", Description = "ObjectDescription")]
		public Guid LogID { get; set; }

		[Display(Name = nameof(StartTime))]
		[DataType(DataType.DateTime)]
		public DateTime StartTime { get; set; }

		[Display(Name = nameof(EndTime))]
		[DataType(DataType.DateTime)]
		public DateTime EndTime { get; set; }

		[Display(Name = nameof(UserID))]
		public Guid UserID { get; set; }

		[Display(Name = nameof(Controller))]
		public string Controller { get; set; }

		[Display(Name = nameof(Action))]
		public string Action { get; set; }

		[Display(Name = nameof(RecordID))]
		public string RecordID { get; set; }

		[Display(Name = nameof(Method))]
		public string Method { get; set; }

		[Display(Name = nameof(QueryString))]
		[DataType(DataType.MultilineText)]
		public string QueryString { get; set; }

		public virtual ICollection<Error> Errors { get; set; }

		public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			return Enumerable.Empty<ValidationResult>();
		}

		public Log(Guid userID)
		{
			UserID = userID;
			Errors = new HashSet<Error>();
		}
	}
}
