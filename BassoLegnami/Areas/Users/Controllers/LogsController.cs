using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BassoLegnami.Model.Data;
using BassoLegnami.Model.Models.Log;
using System.Linq.Expressions;

namespace BassoLegnami.Areas.Users.Controllers
{
	[Area("Users")]
	public class LogsController : BassoLegnami.Controllers.BaseController
	{
		private readonly IUnitOfWork _unitOfWork;

		public LogsController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		// GET: Users/Logs
		public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate, string controllerSearch, string actionSearch, string filter1, string filter2, string filter3, string filter4)
		{
			ViewData["StartDate"] = startDate;
			ViewData["EndDate"] = endDate;
			ViewData["ControllerSearch"] = controllerSearch;
			ViewData["ActionSearch"] = actionSearch;
			ViewData["Filter1"] = filter1;
			ViewData["Filter2"] = filter2;
			ViewData["Filter3"] = filter3;
			ViewData["Filter4"] = filter4;

			if (startDate.HasValue || endDate.HasValue || !string.IsNullOrEmpty(controllerSearch) || !string.IsNullOrEmpty(actionSearch) || !string.IsNullOrEmpty(filter1) || !string.IsNullOrEmpty(filter2) || !string.IsNullOrEmpty(filter3) || !string.IsNullOrEmpty(filter4))
			{
				Expression<Func<Log, bool>> expression = _ => true;
				if (startDate.HasValue)
				{
					expression = expression.And(r => r.StartTime >= startDate);
				}

				if (endDate.HasValue)
				{
					expression = expression.And(r => r.EndTime <= endDate);
				}

				if (!string.IsNullOrEmpty(controllerSearch))
				{
					expression = expression.And(r => r.Controller.Equals(controllerSearch, StringComparison.InvariantCultureIgnoreCase));
				}

				if (!string.IsNullOrEmpty(actionSearch))
				{
					expression = expression.And(r => r.Action.Equals(actionSearch, StringComparison.InvariantCultureIgnoreCase));
				}

				if (!string.IsNullOrEmpty(filter1))
				{
					expression = expression.And(r => !string.IsNullOrEmpty(r.QueryString) && r.QueryString.Contains(filter1, StringComparison.InvariantCultureIgnoreCase));
				}

				if (!string.IsNullOrEmpty(filter2))
				{
					expression = expression.And(r => !string.IsNullOrEmpty(r.QueryString) && r.QueryString.Contains(filter2, StringComparison.InvariantCultureIgnoreCase));
				}

				if (!string.IsNullOrEmpty(filter3))
				{
					expression = expression.And(r => !string.IsNullOrEmpty(r.QueryString) && r.QueryString.Contains(filter3, StringComparison.InvariantCultureIgnoreCase));
				}

				if (!string.IsNullOrEmpty(filter4))
				{
					expression = expression.And(r => !string.IsNullOrEmpty(r.QueryString) && r.QueryString.Contains(filter4, StringComparison.InvariantCultureIgnoreCase));
				}

				return View(await _unitOfWork.LogsRepository.FindByAsync(expression).ConfigureAwait(false));
			}
			return View();
		}

		// GET: Users/Logs/Details/5
		public async Task<IActionResult> Details(Guid? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			Log log = await _unitOfWork.LogsRepository.FindBy(m => m.LogID == id)
				.Include(r => r.Errors)
				.FirstAsync().ConfigureAwait(false);

			if (log == null)
			{
				return NotFound();
			}

			return View(log);
		}
	}
}
