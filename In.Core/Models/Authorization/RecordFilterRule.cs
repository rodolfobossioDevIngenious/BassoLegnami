using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace In.Core.Models.Authorization
{
	public class RecordFilterRule : Auditable
	{
		[Display(Name = "ObjectName", Description = "ObjectDescription")]
		public int RecordFilterRuleID { get; set; }

		[Display(Name = nameof(UserId))]
		public Guid UserId { get; set; }

		[Display(Name = nameof(RecordFilterRuleType))]
		public int RecordFilterRuleTypeID { get; set; }

		[Display(Name = nameof(RecordFilterRuleType))]
		public virtual RecordFilterRuleType RecordFilterRuleType { get; set; }

		public virtual ICollection<RecordFilterRuleValue> RecordFilterRuleValues { get; set; }

		public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			return Enumerable.Empty<ValidationResult>();
		}

		public RecordFilterRule()
		{
			RecordFilterRuleValues = new HashSet<RecordFilterRuleValue>();
		}
	}

	public class RecordFilterRuleType : Auditable
	{
		[Display(Name = "ObjectName", Description = "ObjectDescription")]
		public int RecordFilterRuleTypeID { get; set; }

		[Display(Name = nameof(Name))]
		[StringLength(100)]
		[Required]
		public string Name { get; set; }

		[Display(Name = nameof(Flag1))]
		[StringLength(10)]
		[Required]
		public string Flag1 { get; set; }

		public virtual ICollection<RecordFilterRule> RecordFilterRules { get; set; }

		public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			return Enumerable.Empty<ValidationResult>();
		}

		public RecordFilterRuleType()
		{
			RecordFilterRules = new HashSet<RecordFilterRule>();
		}
	}

	public class RecordFilterRuleValue : Auditable
	{
		[Display(Name = "ObjectName", Description = "ObjectDescription")]
		public int RecordFilterRuleValueID { get; set; }

		[Display(Name = nameof(RecordFilterRule))]
		public int RecordFilterRuleID { get; set; }

		[Display(Name = nameof(RecordID))]
		public int RecordID { get; set; }

		public virtual RecordFilterRule RecordFilterRule { get; set; }

		public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			return Enumerable.Empty<ValidationResult>();
		}
	}
}
