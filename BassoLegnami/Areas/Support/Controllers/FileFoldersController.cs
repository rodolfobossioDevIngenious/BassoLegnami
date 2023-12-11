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
	public class FileFoldersController : BassoLegnami.Controllers.BaseController
	{
		private readonly IUnitOfWork _unitOfWork;

		public FileFoldersController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		// GET: Support/FileFolders
		public async Task<IActionResult> Index()
		{
			return View(await _unitOfWork.FileFoldersRepository.GetAllAsync().ConfigureAwait(false));
		}

		// GET: Support/FileFolders/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			FileFolder fileFolder = await _unitOfWork.FileFoldersRepository.FindAsync(m => m.FileFolderID == id).ConfigureAwait(false);
			if (fileFolder == null)
			{
				return NotFound();
			}

			return View(fileFolder);
		}

		// GET: Support/FileFolders/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Support/FileFolders/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("FileFolderID,Name,Key,Path,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,RowVersion")] FileFolder fileFolder)
		{
			if (ModelState.IsValid)
			{
				_unitOfWork.FileFoldersRepository.Add(fileFolder);
				await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
				return RedirectToAction(nameof(Details), new { id = fileFolder.FileFolderID });
			}
			return View(fileFolder);
		}

		// GET: Support/FileFolders/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			FileFolder fileFolder = await _unitOfWork.FileFoldersRepository.FindAsync(m => m.FileFolderID == id).ConfigureAwait(false);
			if (fileFolder == null)
			{
				return NotFound();
			}

			return View(fileFolder);
		}

		// POST: Support/FileFolders/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("FileFolderID,Name,Key,Path,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,RowVersion")] FileFolder fileFolder)
		{
			if (id != fileFolder.FileFolderID)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_unitOfWork.FileFoldersRepository.Update(fileFolder, id);
					await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!FileFolderExists(fileFolder.FileFolderID))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Details), new { id = fileFolder.FileFolderID });
			}
			return View(fileFolder);
		}

		// GET: Support/FileFolders/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			FileFolder fileFolder = await _unitOfWork.FileFoldersRepository.FindAsync(m => m.FileFolderID == id).ConfigureAwait(false);
			if (fileFolder == null)
			{
				return NotFound();
			}

			return View(fileFolder);
		}

		// POST: Support/FileFolders/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			FileFolder fileFolder = _unitOfWork.FileFoldersRepository.Get(id);
			_unitOfWork.FileFoldersRepository.Delete(fileFolder);
			await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
			return RedirectToAction(nameof(Index));
		}

		private bool FileFolderExists(int id)
		{
			return _unitOfWork.FileFoldersRepository.Any(e => e.FileFolderID == id);
		}
	}
}
