using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BassoLegnami.Model.Models
{
	public partial class EmailMessage : In.Core.Models.Auditable
	{
		public enum EmailMessageStatusEnum
		{
			ToSend = 0,
			NotSended,
			Sended,
		}

		public enum PrioritySendEmailValueEnum
		{
			Hight,
			Medium,
			Low
		}

		public EmailMessage(string recipientEmail, string subject, string message, PrioritySendEmailValueEnum priority) : this()
		{
			RecipientEmail = recipientEmail;
			Subject = subject;
			Message = message;
			PriorityValue = priority;
		}

		public EmailMessage()
		{
			SendDate = null;
			RecipientEmails = new List<string>();
			CCRecipientEmails = new List<string>();
			BCCRecipientEmails = new List<string>();
			Attachments = new HashSet<Support.File>();
		}

		public int EmailMessageID { get; set; }

		[Required(ErrorMessageResourceName = nameof(SharedResource.FieldRequired), ErrorMessageResourceType = typeof(SharedResource))]
		public EmailMessageStatusEnum Status { get; set; }

		[Required(ErrorMessageResourceName = nameof(SharedResource.FieldRequired), ErrorMessageResourceType = typeof(SharedResource))]
		public string RecipientEmail
		{
			get
			{
				if (RecipientEmails.Count > 0)
				{
					return string.Join(";", RecipientEmails);
				}
				return null;
			}
			set
			{
				RecipientEmails.Clear();
				if (!string.IsNullOrEmpty(value))
				{
					RecipientEmails.AddRange(value.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
				}
			}
		}

		public List<string> RecipientEmails { get; }

		public string CCRecipientEmail
		{
			get
			{
				if (CCRecipientEmails.Count > 0)
				{
					return string.Join(";", CCRecipientEmails);
				}
				return null;
			}
			set
			{
				CCRecipientEmails.Clear();
				if (!string.IsNullOrEmpty(value))
				{
					CCRecipientEmails.AddRange(value.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
				}
			}
		}

		public List<string> CCRecipientEmails { get; }

		public string BCCRecipientEmail
		{
			get
			{
				if (BCCRecipientEmails.Count > 0)
				{
					return string.Join(";", BCCRecipientEmails);
				}
				return null;
			}
			set
			{
				BCCRecipientEmails.Clear();
				if (!string.IsNullOrEmpty(value))
				{
					BCCRecipientEmails.AddRange(value.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
				}
			}
		}

		public List<string> BCCRecipientEmails { get; }

		[Required(ErrorMessageResourceName = nameof(SharedResource.FieldRequired), ErrorMessageResourceType = typeof(SharedResource))]
		public string Subject { get; set; }

		public string ErrorMessage { get; set; }
		public DateTime? SendDate { get; set; }

		[Required(ErrorMessageResourceName = nameof(SharedResource.FieldRequired), ErrorMessageResourceType = typeof(SharedResource))]
		public string Message { get; set; }

		public PrioritySendEmailValueEnum PriorityValue { get; set; }

		public virtual ICollection<Support.File> Attachments { get; set; }

		public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			return Enumerable.Empty<ValidationResult>();
		}
	}
}
