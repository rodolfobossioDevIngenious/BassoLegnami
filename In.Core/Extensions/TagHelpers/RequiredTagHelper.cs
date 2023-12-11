using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace In.Core.Extensions.TagHelpers
{
	[HtmlTargetElement("label", Attributes = ForAttributeName)]
	public class RequiredTagHelper : TagHelper
	{
		private const string ForAttributeName = "asp-for";

		[HtmlAttributeName(ForAttributeName)]
		public ModelExpression For { get; set; }

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

			if (For.Metadata.IsRequired && For.ModelExplorer.ModelType != typeof(bool))
			{
				TagHelperAttribute existingClass = output.Attributes.FirstOrDefault(f => f.Name == "class");
				string cssClass = string.Empty;
				if (existingClass != null)
				{
					cssClass = existingClass.Value.ToString();
					output.Attributes.Remove(existingClass);
				}
				cssClass += " required";
				output.Attributes.Add("class", cssClass);
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
