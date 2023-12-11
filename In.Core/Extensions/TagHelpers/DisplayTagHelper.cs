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
    [HtmlTargetElement("display", Attributes = ForAttributeName)]
    public class DisplayTagHelper : InputTagHelper
    {
        private const string ForAttributeName = "asp-for";

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "input";
            if (For.ModelExplorer.ModelType == typeof(bool))
            {
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
            else
            {
                base.Process(context, output);
            }

            TagHelperAttribute existingDisabled = output.Attributes.FirstOrDefault(f => f.Name == "disabled");
            if (existingDisabled == null)
            {
                output.Attributes.Add("disabled", string.Empty);
            }

            if (For.Metadata.DataTypeName?.Equals(nameof(DataType.MultilineText), StringComparison.InvariantCultureIgnoreCase) ?? false)
            {
                output.Content.SetContent(For.Model?.ToString() ?? string.Empty);
            }
            else if (For.Metadata.DataTypeName?.Equals(nameof(DataType.Currency), StringComparison.InvariantCultureIgnoreCase) ?? false)
            {
                output.Attributes.SetAttribute("class", output.Attributes["class"].Value + " text-right");
                TagHelperAttribute existingType = output.Attributes.FirstOrDefault(f => f.Name == "type");
                if (existingType != null)
                {
                    output.Attributes.Remove(existingType);
                }
                output.Attributes.Add("type", "text");
                if (For.Model is float)
                {
                    output.Attributes.Add("value", ((float?)For.Model)?.ToString("c") ?? string.Empty);
                }
                else
                {
                    output.Attributes.Add("value", ((decimal?)For.Model)?.ToString("c") ?? string.Empty);
                }
            }
            else if (For.Metadata.ModelType?.Name.Equals(nameof(DayOfWeek)) ?? false)
            {
                output.Attributes.Add("value", System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetDayName((DayOfWeek)For.Model));
            }
            else if ((For.Metadata.DataTypeName?.Equals(Data.CustomDataTypes.DATA_TYPE_HYPERTEXT, StringComparison.InvariantCultureIgnoreCase) ?? false) || (For.Metadata.DataTypeName?.Equals(Data.CustomDataTypes.DATA_TYPE_SMALL_HYPERTEXT, StringComparison.InvariantCultureIgnoreCase) ?? false))
            {
                output.Attributes.SetAttribute("class", output.Attributes["class"].Value + (For.Metadata.DataTypeName.Equals(Data.CustomDataTypes.DATA_TYPE_HYPERTEXT, StringComparison.InvariantCultureIgnoreCase) ? " hypertext-readonly" : " small-hypertext-readonly"));
            }
            else if (For.Metadata.IsEnum && For.Model != null)
            {
                string t = For.Metadata.EnumGroupedDisplayNamesAndValues.FirstOrDefault(r => r.Value == ((int)For.Model).ToString()).Value;
                if (t == null)
                {
                    output.Attributes.Add("value", For.Model?.ToString() ?? string.Empty);
                }
                else
                {
                    output.Attributes.Add("value", For.Metadata.EnumGroupedDisplayNamesAndValues.FirstOrDefault(r => r.Value == ((int)For.Model).ToString()).Key.Name ?? string.Empty);
                }
            }
            else
            {
                output.Attributes.Add("value", For.Model?.ToString() ?? string.Empty);
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
