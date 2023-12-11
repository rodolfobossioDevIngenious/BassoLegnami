using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace In.Core.Extensions.TagHelpers
{
	[HtmlTargetElement("displayValue", Attributes = ForAttributeName)]
	public class DisplayValueTagHelper : InputTagHelper
	{
		private const string ForAttributeName = "asp-for";

		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			string customType = output.Attributes["data-type"]?.Value.ToString();

			output.TagName = string.Empty;
			if (For.ModelExplorer.ModelType == typeof(bool))
			{
				output.TagName = "input";
				output.Attributes.Add("disabled", "disabled");
				output.Attributes.Add("type", "checkbox");
				output.Attributes.Add("data-toggle", "switch");
				output.Attributes.Add("data-on-color", "info");
				output.Attributes.Add("data-off-color", "info");
				output.Attributes.Add("data-on-text", "<i class='fa fa-check'></i>");
				output.Attributes.Add("data-off-text", "<i class='fa fa-times'></i>");
				output.Content.AppendHtml("<span class='toggle'></span>");
				if (((bool?)For.Model) ?? false)
				{
					output.Attributes.Add("checked", string.Empty);
				}
			}
			else if (customType == nameof(Boolean))
			{
				output.TagName = "input";
				output.Attributes.Add("disabled", "disabled");
				output.Attributes.Add("type", "checkbox");
				output.Attributes.Add("data-toggle", "switch");
				output.Attributes.Add("data-on-color", "info");
				output.Attributes.Add("data-off-color", "info");
				output.Attributes.Add("data-on-text", "<i class='fa fa-check'></i>");
				output.Attributes.Add("data-off-text", "<i class='fa fa-times'></i>");
				output.Content.AppendHtml("<span class='toggle'></span>");
				if ((For.Model?.ToString().Equals("true", StringComparison.InvariantCultureIgnoreCase)) ?? false)
				{
					output.Attributes.Add("checked", string.Empty);
				}
			}
			else if (For.Metadata.DataTypeName?.Equals(nameof(DataType.Date), StringComparison.InvariantCultureIgnoreCase) ?? false)
			{
				output.Content.SetContent(((DateTime?)For.Model)?.ToShortDateString() ?? string.Empty);
			}
			else if (For.Metadata.DataTypeName?.Equals(nameof(DataType.Time), StringComparison.InvariantCultureIgnoreCase) ?? false)
			{
				if (For.Model == null)
				{
					output.Content.SetContent(string.Empty);
				}
				else
				{
					TimeSpan currentTimeSpan = (TimeSpan)For.Model;
					output.Content.SetContent(new TimeSpan(currentTimeSpan.Ticks / TimeSpan.TicksPerSecond * TimeSpan.TicksPerSecond).ToString());
				}
			}
			else if (For.Metadata.DataTypeName?.Equals(nameof(DataType.Currency), StringComparison.InvariantCultureIgnoreCase) ?? false)
			{
				output.Content.SetContent(((decimal?)For.Model)?.ToString("C") ?? string.Empty);
			}
			else if (For.Metadata.ModelType?.Name.Equals(nameof(DayOfWeek)) ?? false)
			{
				output.Content.SetContent(System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetDayName((DayOfWeek)For.Model));
			}
			else if ((Nullable.GetUnderlyingType(For.Metadata.ModelType) ?? For.Metadata.ModelType)?.IsEnum ?? false)
			{
                output.Content.SetContent(((Enum?)For.Model)?.GetEnumDisplayName() ?? string.Empty);
            }
            else
			{
				output.Content.SetContent(For.Model?.ToString() ?? string.Empty);
			}
		}

		public override int Order
		{
			get
			{
				return 100; // Needs to run after aspnet
			}
		}
	}
}
