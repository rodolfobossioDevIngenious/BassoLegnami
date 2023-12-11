using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace In.Core.Extensions.TagHelpers
{
	[HtmlTargetElement("input", Attributes = ForAttributeName)]
	public class InputTagHelper : TagHelper
	{
		private const string ForAttributeName = "asp-for";

		[HtmlAttributeName(ForAttributeName)]
		public ModelExpression For { get; set; }

		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			bool isHidden = output.Attributes["type"]?.Value.Equals("hidden") ?? false;
			string customType = output.Attributes["data-type"]?.Value.ToString();

			if (context == null)
			{
				throw new ArgumentNullException(nameof(context));
			}

			if (output == null)
			{
				throw new ArgumentNullException(nameof(output));
			}

			if (For.Metadata.IsRequired && !output.Attributes.Any(a => a.Name == "required") && For.ModelExplorer.ModelType != typeof(bool))
			{
				output.Attributes.SetAttribute("required", "");
			}

			if (For.Metadata.ContainerType.GetProperty(For.Name)?.GetCustomAttribute(typeof(StringLengthAttribute)) is StringLengthAttribute stringLength)
			{
				output.Attributes.SetAttribute("maxlength", stringLength.MaximumLength);
			}

			TagHelperAttribute existingClass = output.Attributes.FirstOrDefault(f => f.Name == "class");
			string cssClass = string.Empty;
			if (existingClass != null)
			{
				if (existingClass.Value.GetType().Name == nameof(HtmlString))
				{
					cssClass = existingClass.Value.ToString();
				}
			}
			cssClass += " form-control";

			if (!isHidden)
			{
				if (For.Metadata.DataTypeName?.Equals(nameof(DataType.MultilineText), StringComparison.InvariantCultureIgnoreCase) ?? false)
				{
					output.TagName = "textarea";
					output.TagMode = TagMode.StartTagAndEndTag;
					output.Content.SetContent(For.Model?.ToString() ?? string.Empty);
				}
				else if ((For.Metadata.DataTypeName?.Equals(nameof(DataType.Date), StringComparison.InvariantCultureIgnoreCase) ?? false) || (customType?.Equals(nameof(DataType.Date)) ?? false))
				{
					TagHelperAttribute existingType = output.Attributes.FirstOrDefault(f => f.Name == "type");
					if (existingType != null)
					{
						output.Attributes.Remove(existingType);
					}
					if (For.Model as DateTime? == DateTime.MinValue)
					{
						output.Attributes.SetAttribute(new TagHelperAttribute("value", string.Empty));
					}

					if (!string.IsNullOrEmpty(customType))
					{
						cssClass += " lineardatepicker";
					}
					else
					{
						cssClass += " datepicker";
					}
				}
				else if (For.Metadata.DataTypeName?.Equals(nameof(DataType.Time), StringComparison.InvariantCultureIgnoreCase) ?? false)
				{
					TagHelperAttribute existingType = output.Attributes.FirstOrDefault(f => f.Name == "type");
					if (existingType != null)
					{
						output.Attributes.Remove(existingType);
					}

					cssClass += " timepicker";
				}
				else if ((For.Metadata.DataTypeName?.Equals(nameof(DataType.DateTime), StringComparison.InvariantCultureIgnoreCase) ?? false) || (customType?.Equals(nameof(DataType.DateTime)) ?? false))
				{
					TagHelperAttribute existingType = output.Attributes.FirstOrDefault(f => f.Name == "type");
					if (existingType != null)
					{
						output.Attributes.Remove(existingType);
					}

					cssClass += " datetimepicker";
				}
				else if (For.Metadata.DataTypeName?.Equals("color", StringComparison.InvariantCultureIgnoreCase) ?? false)
				{
					cssClass += " minicolors";
					output.Attributes.Add("data-position", "bottom right");
				}
				else if ((For.Metadata.DataTypeName?.Equals(nameof(DataType.Currency), StringComparison.InvariantCultureIgnoreCase) ?? false) || (customType?.Equals("Decimal") ?? false) || (customType?.Equals("Int") ?? false))
				{
					cssClass += " text-right";
					if (output.Attributes.ContainsName("type"))
					{
						output.Attributes.Remove(output.Attributes["type"]);
					}

					decimal temp;
					// typecast either 'temp' or 'null'
					decimal? value = decimal.TryParse(For.Model?.ToString(), out temp) ? temp : null;
					string invariantValue = value?.ToString(System.Globalization.CultureInfo.InvariantCulture);
					output.Attributes.SetAttribute(new TagHelperAttribute("value", invariantValue));

					output.Attributes.Add("type", "number");
				}
				else if ((For.Metadata.DataTypeName?.Equals(Data.CustomDataTypes.DATA_TYPE_HYPERTEXT, StringComparison.InvariantCultureIgnoreCase) ?? false) || (For.Metadata.DataTypeName?.Equals(Data.CustomDataTypes.DATA_TYPE_SMALL_HYPERTEXT, StringComparison.InvariantCultureIgnoreCase) ?? false))
				{
					output.TagName = "textarea";
					output.TagMode = TagMode.StartTagAndEndTag;
					output.Content.SetContent(For.Model?.ToString() ?? string.Empty);
					cssClass += For.Metadata.DataTypeName.Equals(Data.CustomDataTypes.DATA_TYPE_HYPERTEXT, StringComparison.InvariantCultureIgnoreCase) ? " hypertext" : " small-hypertext";
				}
			}

			if (existingClass != null)
			{
				output.Attributes.Remove(existingClass);
			}

			output.Attributes.Add("class", cssClass);

			if (!isHidden && For.ModelExplorer.ModelType == typeof(bool))
			{
				if (output.Attributes.ContainsName("type"))
				{
					output.Attributes.Remove(output.Attributes["type"]);
				}

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

			if (!isHidden && customType == nameof(Boolean))
			{
				if (output.Attributes.ContainsName("type"))
				{
					output.Attributes.Remove(output.Attributes["type"]);
				}

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
