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
	public class FestivitiesController : BassoLegnami.Controllers.BaseController
	{
		private readonly IUnitOfWork _unitOfWork;

		public FestivitiesController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		// GET: Support/Festivities
		public async Task<IActionResult> Index()
		{
			return View(await _unitOfWork.FestivitiesRepository.GetAllAsync().ConfigureAwait(false));
		}

		// GET: Support/Festivities/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			Festivity festivity = await _unitOfWork.FestivitiesRepository.FindAsync(m => m.FestivityID == id).ConfigureAwait(false);
			if (festivity == null)
			{
				return NotFound();
			}

			return View(festivity);
		}

		// GET: Support/Festivities/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Support/Festivities/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("FestivityID,Name,Day,Month,Year,City,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,RowVersion")] Festivity festivity)
		{
			if (ModelState.IsValid)
			{
				_unitOfWork.FestivitiesRepository.Add(festivity);
				await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
				return RedirectToAction(nameof(Details), new { id = festivity.FestivityID });
			}
			return View(festivity);
		}

		// GET: Support/Festivities/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			Festivity festivity = await _unitOfWork.FestivitiesRepository.FindAsync(m => m.FestivityID == id).ConfigureAwait(false);
			if (festivity == null)
			{
				return NotFound();
			}

			return View(festivity);
		}

		// POST: Support/Festivities/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("FestivityID,Name,Day,Month,Year,City,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,RowVersion")] Festivity festivity)
		{
			if (id != festivity.FestivityID)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_unitOfWork.FestivitiesRepository.Update(festivity, id);
					await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!FestivityExists(festivity.FestivityID))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Details), new { id = festivity.FestivityID });
			}
			return View(festivity);
		}

		// GET: Support/Festivities/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			Festivity festivity = await _unitOfWork.FestivitiesRepository.FindAsync(m => m.FestivityID == id).ConfigureAwait(false);
			if (festivity == null)
			{
				return NotFound();
			}

			return View(festivity);
		}

		// POST: Support/Festivities/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			Festivity festivity = _unitOfWork.FestivitiesRepository.Get(id);
			_unitOfWork.FestivitiesRepository.Delete(festivity);
			await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
			return RedirectToAction(nameof(Index));
		}

		private bool FestivityExists(int id)
		{
			return _unitOfWork.FestivitiesRepository.Any(e => e.FestivityID == id);
		}
	}
}
