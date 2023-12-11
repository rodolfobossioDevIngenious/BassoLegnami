using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using In.Core.Models.Authorization;
using BassoLegnami.Model.Data;

namespace BassoLegnami.Areas.Users.Controllers
{
	[Area("Users")]
	public class RecordFilterRuleTypesController : BassoLegnami.Controllers.BaseController
	{
		private readonly IUnitOfWork _unitOfWork;

		public RecordFilterRuleTypesController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		// GET: Users/RecordFilterRuleTypes
		public async Task<IActionResult> Index()
		{
			return View(await _unitOfWork.RecordFilterRuleTypesRepository.GetAllAsync().ConfigureAwait(false));
		}

		// GET: Users/RecordFilterRuleTypes/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			RecordFilterRuleType recordFilterRuleType = await _unitOfWork.RecordFilterRuleTypesRepository.FindAsync(m => m.RecordFilterRuleTypeID == id).ConfigureAwait(false);
			if (recordFilterRuleType == null)
			{
				return NotFound();
			}

			return View(recordFilterRuleType);
		}

		// GET: Users/RecordFilterRuleTypes/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Users/RecordFilterRuleTypes/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("RecordFilterRuleTypeID,Name,Flag1,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,RowVersion")] RecordFilterRuleType recordFilterRuleType)
		{
			if (ModelState.IsValid)
			{
				_unitOfWork.RecordFilterRuleTypesRepository.Add(recordFilterRuleType);
				await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
				return RedirectToAction(nameof(Details), new { id = recordFilterRuleType.RecordFilterRuleTypeID });
			}
			return View(recordFilterRuleType);
		}

		// GET: Users/RecordFilterRuleTypes/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			RecordFilterRuleType recordFilterRuleType = await _unitOfWork.RecordFilterRuleTypesRepository.FindAsync(m => m.RecordFilterRuleTypeID == id).ConfigureAwait(false);
			if (recordFilterRuleType == null)
			{
				return NotFound();
			}

			return View(recordFilterRuleType);
		}

		// POST: Users/RecordFilterRuleTypes/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("RecordFilterRuleTypeID,Name,Flag1,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,RowVersion")] RecordFilterRuleType recordFilterRuleType)
		{
			if (id != recordFilterRuleType.RecordFilterRuleTypeID)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_unitOfWork.RecordFilterRuleTypesRepository.Update(recordFilterRuleType, id);
					await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!RecordFilterRuleTypeExists(recordFilterRuleType.RecordFilterRuleTypeID))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Details), new { id = recordFilterRuleType.RecordFilterRuleTypeID });
			}
			return View(recordFilterRuleType);
		}

		// GET: Users/RecordFilterRuleTypes/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			RecordFilterRuleType recordFilterRuleType = await _unitOfWork.RecordFilterRuleTypesRepository.FindAsync(m => m.RecordFilterRuleTypeID == id).ConfigureAwait(false);
			if (recordFilterRuleType == null)
			{
				return NotFound();
			}

			return View(recordFilterRuleType);
		}

		// POST: Users/RecordFilterRuleTypes/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			RecordFilterRuleType recordFilterRuleType = _unitOfWork.RecordFilterRuleTypesRepository.Get(id);
			_unitOfWork.RecordFilterRuleTypesRepository.Delete(recordFilterRuleType);
			await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
			return RedirectToAction(nameof(Index));
		}

		private bool RecordFilterRuleTypeExists(int id)
		{
			return _unitOfWork.RecordFilterRuleTypesRepository.Any(e => e.RecordFilterRuleTypeID == id);
		}
	}
}
