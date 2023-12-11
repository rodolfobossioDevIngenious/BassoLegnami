using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Microsoft.AspNetCore.Mvc.Rendering
{
	public static class HtmlExtensions
	{
		public static IHtmlContent DescriptionFor<TModel, TValue>(this IHtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
		{
			if (!(expression.Body is MemberExpression))
			{
				return new HtmlString(string.Empty);
			}

			ModelExpressionProvider modelExpressionProvider = (ModelExpressionProvider)html.ViewContext.HttpContext.RequestServices.GetService(typeof(IModelExpressionProvider));
			ModelExpression modelExplorer = modelExpressionProvider.CreateModelExpression(html.ViewData, expression);
			if (modelExplorer == null)
			{
				throw new InvalidOperationException($"Failed to get model explorer for {modelExpressionProvider.GetExpressionText(expression)}");
			}
				
			return new HtmlString(modelExplorer.Metadata.Description);
		}
    }
}