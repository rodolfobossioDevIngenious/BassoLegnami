using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace In.Core.Models
{
	public abstract class Auditable : IValidatableObject, IDisposable
	{
		public string CreatedBy { get; set; }
		public DateTime? CreatedOn { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime? UpdatedOn { get; set; }

		[Timestamp]
		public byte[] RowVersion { get; set; }

		public abstract IEnumerable<ValidationResult> Validate(ValidationContext validationContext);

		public virtual void Dispose()
		{
		}
	}
}
