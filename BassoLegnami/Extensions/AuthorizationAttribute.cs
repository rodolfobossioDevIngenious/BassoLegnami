using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BassoLegnami.Extensions
{
	public class AuthorizationAttribute : TypeFilterAttribute
	{
		public AuthorizationAttribute() : base(typeof(AuthorizationAttributeImpl))
		{
		}

		private class AuthorizationAttributeImpl : IActionFilter
		{
			private readonly Model.Data.IUnitOfWork _unitOfWork;

			public AuthorizationAttributeImpl(Model.Data.IUnitOfWork unitOfWork)
			{
				_unitOfWork = unitOfWork;
			}

			public void OnActionExecuting(ActionExecutingContext context)
			{
				string controller = context.RouteData.Values["controller"].ToString();
				string action = context.RouteData.Values["action"].ToString();

				bool isAuthenticated = context?.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
				if (isAuthenticated && !_unitOfWork.AuthorizationsRepository.IsAuthorized(controller, action) && (!(controller.Equals("account", StringComparison.InvariantCultureIgnoreCase) || controller.Equals("manage", StringComparison.InvariantCultureIgnoreCase))))
				{
					context.Result = new EmptyResult();
				}
			}

			public void OnActionExecuted(ActionExecutedContext context)
			{
			}
		}
	}
}
