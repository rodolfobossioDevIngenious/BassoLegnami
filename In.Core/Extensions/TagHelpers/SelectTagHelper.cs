using Microsoft.AspNetCore.Mvc.Rendering;
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
	[HtmlTargetElement("Select", Attributes = ForAttributeName)]
	public class SelectTagHelper : TagHelper
	{
		private const string ForAttributeName = "asp-for";

		private IHtmlHelper _htmlHelper;

		[HtmlAttributeName(ForAttributeName)]
		public ModelExpression For { get; set; }

		public SelectTagHelper(IHtmlHelper htmlHelper)
		{
			_htmlHelper = htmlHelper;
		}

		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			if (context == null)
			{
				throw new ArgumentNullException(nameof(context));
			}

			if (output == null)
			{
				throw new ArgumentNullException(nameof(output));
			}

			if (For.Metadata.IsEnum)
			{
				IEnumerable<SelectListItem> options = _htmlHelper.GetEnumSelectList(For.Metadata.ModelType);
				output.Content.Clear();
				output.Content.AppendHtml($"<option value='' {(For.Model == null ? string.Empty : "selected")}></option>");
				foreach (SelectListItem option in options)
				{
					bool selected = false;
					if (For.Model != null)
					{
						selected = ((int)(For.Model ?? -1)).ToString() == option.Value;
					}
					output.Content.AppendHtml($"<option value='{option.Value}' {(selected ? "selected" : string.Empty)}>{option.Text}</option>");
				}
			}
			else
			{
				output.Content.AppendHtml("<option value='' disabled selected></option>");
			}

			if (For.Metadata.IsRequired && !output.Attributes.Any(a => a.Name == "required") && For.ModelExplorer.ModelType != typeof(bool))
			{
				output.Attributes.SetAttribute("required", "");
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
