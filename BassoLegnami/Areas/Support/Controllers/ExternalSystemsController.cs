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
	public class ExternalSystemsController : BassoLegnami.Controllers.BaseController
	{
		private readonly IUnitOfWork _unitOfWork;

		public ExternalSystemsController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		// GET: Support/ExternalSystems
		public async Task<IActionResult> Index()
		{
			return View(await _unitOfWork.ExternalSystemsRepository.GetAllAsync().ConfigureAwait(false));
		}

		// GET: Support/ExternalSystems/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			ExternalSystem externalSystem = await _unitOfWork.ExternalSystemsRepository.FindAsync(m => m.ExternalSystemID == id).ConfigureAwait(false);
			if (externalSystem == null)
			{
				return NotFound();
			}

			return View(externalSystem);
		}

		// GET: Support/ExternalSystems/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Support/ExternalSystems/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("ExternalSystemID,Name,Key,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,RowVersion")] ExternalSystem externalSystem)
		{
			if (ModelState.IsValid)
			{
				_unitOfWork.ExternalSystemsRepository.Add(externalSystem);
				await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
				return RedirectToAction(nameof(Details), new { id = externalSystem.ExternalSystemID });
			}
			return View(externalSystem);
		}

		// GET: Support/ExternalSystems/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			ExternalSystem externalSystem = await _unitOfWork.ExternalSystemsRepository.FindAsync(m => m.ExternalSystemID == id).ConfigureAwait(false);
			if (externalSystem == null)
			{
				return NotFound();
			}

			return View(externalSystem);
		}

		// POST: Support/ExternalSystems/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("ExternalSystemID,Name,Key,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,RowVersion")] ExternalSystem externalSystem)
		{
			if (id != externalSystem.ExternalSystemID)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_unitOfWork.ExternalSystemsRepository.Update(externalSystem, id);
					await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!ExternalSystemExists(externalSystem.ExternalSystemID))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Details), new { id = externalSystem.ExternalSystemID });
			}
			return View(externalSystem);
		}

		// GET: Support/ExternalSystems/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			ExternalSystem externalSystem = await _unitOfWork.ExternalSystemsRepository.FindAsync(m => m.ExternalSystemID == id).ConfigureAwait(false);
			if (externalSystem == null)
			{
				return NotFound();
			}

			return View(externalSystem);
		}

		// POST: Support/ExternalSystems/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			ExternalSystem externalSystem = _unitOfWork.ExternalSystemsRepository.Get(id);
			_unitOfWork.ExternalSystemsRepository.Delete(externalSystem);
			await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
			return RedirectToAction(nameof(Index));
		}

		private bool ExternalSystemExists(int id)
		{
			return _unitOfWork.ExternalSystemsRepository.Any(e => e.ExternalSystemID == id);
		}
	}
}
