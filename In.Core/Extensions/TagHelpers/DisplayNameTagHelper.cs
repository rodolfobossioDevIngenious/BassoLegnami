using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace In.Core.Extensions.TagHelpers
{
	[HtmlTargetElement("displayName", Attributes = ForAttributeName)]
	public class DisplayNameTagHelper : InputTagHelper
	{
		private const string ForAttributeName = "asp-for";

		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			output.TagName = string.Empty;
			output.Content.SetContent(For.Metadata.DisplayName);
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
