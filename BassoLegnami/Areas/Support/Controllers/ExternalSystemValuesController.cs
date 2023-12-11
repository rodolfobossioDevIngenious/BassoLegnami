using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BassoLegnami.Model.Data;
using BassoLegnami.Model.Models.Support;

namespace BassoLegnami.Areas.Support.Controllers
{
	[Area("Support")]
	public class ExternalSystemValuesController : BassoLegnami.Controllers.BaseController
	{
		private readonly IUnitOfWork _unitOfWork;

		public ExternalSystemValuesController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		// GET: Support/ExternalSystemValues
		public async Task<IActionResult> Index()
		{
			return View(await _unitOfWork.ExternalSystemValuesRepository.GetAllAsyncIncluding(r => r.ExternalSystem).ConfigureAwait(false));
		}

		// GET: Support/ExternalSystemValues/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			ExternalSystemValue externalSystemValue = await _unitOfWork.ExternalSystemValuesRepository.FindAsync(m => m.ExternalSystemValueID == id, r => r.ExternalSystem).ConfigureAwait(false);
			if (externalSystemValue == null)
			{
				return NotFound();
			}

			return View(externalSystemValue);
		}

		// GET: Support/ExternalSystemValues/Create
		public IActionResult Create()
		{
			ViewData["ExternalSystemID"] = new SelectList(_unitOfWork.ExternalSystemsRepository.GetAll(), "ExternalSystemID", "Name");
			return View();
		}

		// POST: Support/ExternalSystemValues/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("ExternalSystemValueID,ExternalSystemID,Value1,Value2,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,RowVersion")] ExternalSystemValue externalSystemValue)
		{
			if (ModelState.IsValid)
			{
				_unitOfWork.ExternalSystemValuesRepository.Add(externalSystemValue);
				await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
				return RedirectToAction(nameof(Details), new { id = externalSystemValue.ExternalSystemValueID });
			}
			ViewData["ExternalSystemID"] = new SelectList(_unitOfWork.ExternalSystemsRepository.GetAll(), "ExternalSystemID", "Name", externalSystemValue.ExternalSystemID);
			return View(externalSystemValue);
		}

		// GET: Support/ExternalSystemValues/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			ExternalSystemValue externalSystemValue = await _unitOfWork.ExternalSystemValuesRepository.FindAsync(m => m.ExternalSystemValueID == id, r => r.ExternalSystem).ConfigureAwait(false);
			if (externalSystemValue == null)
			{
				return NotFound();
			}

			ViewData["ExternalSystemID"] = new SelectList(_unitOfWork.ExternalSystemsRepository.GetAll(), "ExternalSystemID", "Name", externalSystemValue.ExternalSystemID);
			return View(externalSystemValue);
		}

		// POST: Support/ExternalSystemValues/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("ExternalSystemValueID,ExternalSystemID,Value1,Value2,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,RowVersion")] ExternalSystemValue externalSystemValue)
		{
			if (id != externalSystemValue.ExternalSystemValueID)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_unitOfWork.ExternalSystemValuesRepository.Update(externalSystemValue, id);
					await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!ExternalSystemValueExists(externalSystemValue.ExternalSystemValueID))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Details), new { id = externalSystemValue.ExternalSystemValueID });
			}
			ViewData["ExternalSystemID"] = new SelectList(_unitOfWork.ExternalSystemsRepository.GetAll(), "ExternalSystemID", "Name", externalSystemValue.ExternalSystemID);
			return View(externalSystemValue);
		}

		// GET: Support/ExternalSystemValues/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			ExternalSystemValue externalSystemValue = await _unitOfWork.ExternalSystemValuesRepository.FindAsync(m => m.ExternalSystemValueID == id, r => r.ExternalSystem).ConfigureAwait(false);
			if (externalSystemValue == null)
			{
				return NotFound();
			}

			return View(externalSystemValue);
		}

		// POST: Support/ExternalSystemValues/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			ExternalSystemValue externalSystemValue = _unitOfWork.ExternalSystemValuesRepository.Get(id);
			_unitOfWork.ExternalSystemValuesRepository.Delete(externalSystemValue);
			await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
			return RedirectToAction(nameof(Index));
		}

		private bool ExternalSystemValueExists(int id)
		{
			return _unitOfWork.ExternalSystemValuesRepository.Any(e => e.ExternalSystemValueID == id);
		}
	}
}
