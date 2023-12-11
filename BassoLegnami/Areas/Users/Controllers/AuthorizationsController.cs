using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using In.Core.Models.Authorization;
using BassoLegnami.Model.Data;
using Microsoft.AspNetCore.Identity;

namespace BassoLegnami.Areas.Users.Controllers
{
	[Area("Users")]
	public class AuthorizationsController : BassoLegnami.Controllers.BaseController
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly RoleManager<IdentityRole> _roleManager;

		public AuthorizationsController(IUnitOfWork unitOfWork, RoleManager<IdentityRole> roleManager)
		{
			_unitOfWork = unitOfWork;
			_roleManager = roleManager;
		}

		// GET: Users/Authorizations
		public async Task<IActionResult> Index()
		{
			return View(await _unitOfWork.AuthorizationsRepository.GetAllAsync().ConfigureAwait(false));
		}

		// GET: Users/Authorizations/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			Authorization authorization = await _unitOfWork.AuthorizationsRepository.FindAsync(m => m.AuthorizationID == id).ConfigureAwait(false);
			if (authorization == null)
			{
				return NotFound();
			}

			return View(authorization);
		}

		// GET: Users/Authorizations/Create
		public IActionResult Create()
		{
			ViewData["RoleId"] = new SelectList(_roleManager.Roles.Select(r => new SelectListItem(r.Name, r.Id)), "Value", "Text");
			return View();
		}

		// POST: Users/Authorizations/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("AuthorizationID,RoleId,Controller,Action,Authorized,Note,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,RowVersion")] Authorization authorization)
		{
			if (ModelState.IsValid)
			{
				_unitOfWork.AuthorizationsRepository.Add(authorization);
				await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
				_unitOfWork.AuthorizationsRepository.Flush();
				return RedirectToAction(nameof(Details), new { id = authorization.AuthorizationID });
			}
			ViewData["RoleId"] = new SelectList(_roleManager.Roles.Select(r => new SelectListItem(r.Name, r.Id)), "Value", "Text", authorization.RoleId);
			return View(authorization);
		}

		// GET: Users/Authorizations/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			Authorization authorization = await _unitOfWork.AuthorizationsRepository.FindAsync(m => m.AuthorizationID == id).ConfigureAwait(false);
			if (authorization == null)
			{
				return NotFound();
			}

			ViewData["RoleId"] = new SelectList(_roleManager.Roles.Select(r => new SelectListItem(r.Name, r.Id)), "Value", "Text", authorization.RoleId);
			return View(authorization);
		}

		// POST: Users/Authorizations/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("AuthorizationID,RoleId,Controller,Action,Authorized,Note,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,RowVersion")] Authorization authorization)
		{
			if (id != authorization.AuthorizationID)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_unitOfWork.AuthorizationsRepository.Update(authorization, id);
					await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
					_unitOfWork.AuthorizationsRepository.Flush();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!AuthorizationExists(authorization.AuthorizationID))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Details), new { id = authorization.AuthorizationID });
			}

			ViewData["RoleId"] = new SelectList(_roleManager.Roles.Select(r => new SelectListItem(r.Name, r.Id)), "Value", "Text", authorization.RoleId);
			return View(authorization);
		}

		// GET: Users/Authorizations/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			Authorization authorization = await _unitOfWork.AuthorizationsRepository.FindAsync(m => m.AuthorizationID == id).ConfigureAwait(false);
			if (authorization == null)
			{
				return NotFound();
			}

			return View(authorization);
		}

		// POST: Users/Authorizations/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			Authorization authorization = _unitOfWork.AuthorizationsRepository.Get(id);
			_unitOfWork.AuthorizationsRepository.Delete(authorization);
			await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
			_unitOfWork.AuthorizationsRepository.Flush();
			return RedirectToAction(nameof(Index));
		}

		private bool AuthorizationExists(int id)
		{
			return _unitOfWork.AuthorizationsRepository.Any(e => e.AuthorizationID == id);
		}
	}
}
