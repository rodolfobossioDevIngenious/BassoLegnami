using In.Core.Configuration;
using BassoLegnami.Model.Data;
using In.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BassoLegnami.ActionFilters
{
    public class LogActionFilter : IAsyncActionFilter, IAsyncExceptionFilter
    {
        private const string JSON_MEDIA_TYPE = "application/json";
        private const string CHANGE_PASSWORD_CONTROLLER = "Manage";
        private const string CHANGE_PASSWORD_ACTION = "ChangePassword";
        private const string LOGOUT_CONTROLLER = "Account";
        private const string LOGOUT_ACTION = "Logout";

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOptions<ApplicationConfigurations> _applicationConfigurations;
        private readonly IUnitOfWork _unitOfWork;
        private readonly SignInManager<ApplicationUser> _signInManager;
        protected Model.Models.Log.Log _lastLog;

        public LogActionFilter(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IOptions<ApplicationConfigurations> applicationConfigurations, SignInManager<ApplicationUser> signInManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _applicationConfigurations = applicationConfigurations;
            _signInManager = signInManager;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request?.ContentType?.Contains(JSON_MEDIA_TYPE, StringComparison.InvariantCultureIgnoreCase) ?? true)
            {
                if (_unitOfWork.User != default(Guid))
                {
                    string controller = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ControllerName?.ToLower();
                    string action = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ActionName?.ToLower();
                    ApplicationUser user = await _userManager.FindByIdAsync(_unitOfWork.User.ToString()).ConfigureAwait(false);

                    if (user.Enabled)
                    {
                        bool changePassword = user.ChangePassword;
                        changePassword = changePassword || ((user.LastPasswordChangedDate ?? DateTime.MinValue).AddDays(_applicationConfigurations.Value.PasswordExpireDays) < DateTime.Today);
                        changePassword = changePassword && !await _userManager.IsInRoleAsync(user, In.Core.Models.Authorization.Authorization.ROLE_ADMINISTRATORS).ConfigureAwait(false);
                        if (changePassword && !controller.Equals(CHANGE_PASSWORD_CONTROLLER, StringComparison.InvariantCultureIgnoreCase) && !action.Equals(CHANGE_PASSWORD_ACTION, StringComparison.InvariantCultureIgnoreCase) &&
                             !controller.Equals(LOGOUT_CONTROLLER, StringComparison.InvariantCultureIgnoreCase) && !action.Equals(LOGOUT_ACTION, StringComparison.InvariantCultureIgnoreCase))
                        {
                            context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                            {
                                controller = CHANGE_PASSWORD_CONTROLLER,
                                action = CHANGE_PASSWORD_ACTION,
                                area = string.Empty,
                                code = user.Id,
                            }));
                            return;
                        }

                        string queryString = string.Empty;
                        IEnumerable<KeyValuePair<string, Microsoft.Extensions.Primitives.StringValues>> collection;
                        if (context.HttpContext.Request.ContentType?.Contains("application/json") ?? false)
                        {
                            if (context.ActionArguments.Count > 0)
                            {
                                queryString = Newtonsoft.Json.JsonConvert.SerializeObject(context.ActionArguments.ElementAt(0).Value);
                            }
                        }
                        else
                        {
                            try
                            {
                                collection = context.HttpContext.Request.ContentLength.HasValue ? context.HttpContext.Request.Form : (IEnumerable<KeyValuePair<string, Microsoft.Extensions.Primitives.StringValues>>)context.HttpContext.Request.Query;
                                collection.ToList().ForEach(r => queryString += $"{r.Key}={r.Value}&");
                            }
                            catch
                            {
                                // nothing to do
                            }
                        }
                        _lastLog = new (_unitOfWork.User)
                        {
                            LogID = Guid.NewGuid(),
                            StartTime = DateTime.Now,
                            Controller = controller,
                            Action = action,
                            RecordID = (string)context.RouteData.Values["id"],
                            Method = context.HttpContext.Request.Method,
                            QueryString = string.IsNullOrEmpty(queryString) ? null : queryString,
                        };
                        await next();
                    }
                    else
                    {
                        await _signInManager.SignOutAsync().ConfigureAwait(false);
                        context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                        {
                            controller = "Account",
                            action = "Login",
                            area = string.Empty,
                        }));
                    }
                }
            }

            if (!context.HttpContext.Request?.ContentType?.Contains(JSON_MEDIA_TYPE, StringComparison.InvariantCultureIgnoreCase) ?? true)
            {
                if (_lastLog != null)
                {
                    _lastLog.EndTime = DateTime.Now;
                    _unitOfWork.LogsRepository.Add(_lastLog);
                    await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
                }
            }
        }

        public async Task OnExceptionAsync(ExceptionContext context)
        {
            if (!context.HttpContext.Request?.ContentType?.Contains(JSON_MEDIA_TYPE, StringComparison.InvariantCultureIgnoreCase) ?? true)
            {
                if (_lastLog != null)
                {
                    Model.Models.Log.Error error = new Model.Models.Log.Error()
                    {
                        ErrorID = Guid.NewGuid(),
                        ExceptionName = context.Exception.GetType().FullName,
                        Message = context.Exception.GetFullMessage(),
                        StackTrace = context.Exception.GetFullStackTrace(),
                        Source = context.Exception.Source,
                        LogID = _lastLog.LogID
                    };
                    _unitOfWork.ErrorsRepository.Add(error);
                    await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
                }
            }
        }
    }
}
public static class ExceptionExtensions
{
    public static string GetFullMessage(this Exception ex)
    {
        return ex.InnerException == null
             ? ex.Message
             : ex.Message + " --> " + ex.InnerException.GetFullMessage();
    }

    public static string GetFullStackTrace(this Exception ex)
    {
        return ex.InnerException == null
             ? ex.StackTrace
             : ex.StackTrace + " --> " + ex.InnerException.GetFullStackTrace();
    }
}