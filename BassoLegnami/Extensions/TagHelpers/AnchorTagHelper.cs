using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BassoLegnami.Extensions.TagHelpers
{
	[HtmlTargetElement("a")]
	public class AnchorTagHelper : Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper
	{
		private readonly Model.Data.IUnitOfWork _unitOfWork;

		public AnchorTagHelper(IHtmlGenerator generator, Model.Data.IUnitOfWork unitOfWork)
			: base(generator)
		{
			_unitOfWork = unitOfWork;
		}

		public bool Forced { get; set; }

		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			string pageController = ViewContext.RouteData.Values["controller"] as string;
			string pageAction = ViewContext.RouteData.Values["action"] as string;
			if (!Forced && !_unitOfWork.AuthorizationsRepository.IsAuthorized(Controller ?? pageController, Action ?? pageAction))
			{
				output.SuppressOutput();
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
