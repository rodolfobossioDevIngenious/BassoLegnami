using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BassoLegnami.Model.Data;
using BassoLegnami.Model.Models.Users;
using Microsoft.AspNetCore.Identity;
using In.Core.Models;

namespace BassoLegnami.Areas.Roles.Controllers
{
	[Area("Users")]
	public class RolesController : BassoLegnami.Controllers.BaseController
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;

		public RolesController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			_userManager = userManager;
			_roleManager = roleManager;
		}

		// GET: Roles/Roles
		public IActionResult Index()
		{
			return View(_roleManager.Roles.Select(r => new Role()
			{
				RoleId = r.Id,
				Name = r.Name,
			}));
		}

		// GET: Roles/Roles/Details/5
		public async Task<IActionResult> Details(string id)
		{
			if (id == null)
			{
				return NotFound();
			}

			IdentityRole role = await _roleManager.Roles
				.Where(r => r.Id == id)
				.FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false);
			if (role == null)
			{
				return NotFound();
			}

			IList<ApplicationUser> roleUsers = await _userManager.GetUsersInRoleAsync(role.Name).ConfigureAwait(false);
			return View(new Role()
			{
				RoleId = role.Id,
				Name = role.Name,
			});
		}

		// GET: Roles/Roles/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Roles/Roles/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("RoleId,Name")] Role role)
		{
			if (ModelState.IsValid)
			{
				IdentityResult createRoleTask = await _roleManager.CreateAsync(new IdentityRole
				{
					Name = role.Name
				}).ConfigureAwait(false);
				string roleId = (await _roleManager.FindByNameAsync(role.Name).ConfigureAwait(false)).Id;
				return RedirectToAction(nameof(Details), new { id = roleId });
			}
			return View(role);
		}

		// GET: Roles/Roles/Edit/5
		public async Task<IActionResult> Edit(string id)
		{
			if (id == null)
			{
				return NotFound();
			}

			IdentityRole role = await _roleManager.Roles.FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false);
			if (role == null)
			{
				return NotFound();
			}

			return View(new Role()
			{
				RoleId = role.Id,
				Name = role.Name
			});
		}

		// POST: Roles/Roles/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(string id, [Bind("RoleId,Name")] Role role)
		{
			if (id != role.RoleId)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					IdentityRole t = await _roleManager.FindByIdAsync(role.RoleId).ConfigureAwait(false);
					t.Name = role.Name;
					IdentityResult userTask = await _roleManager.UpdateAsync(t).ConfigureAwait(false);
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!RoleExists(role.RoleId))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Details), new { id = role.RoleId });
			}
			return View(role);
		}

		// GET: Roles/Roles/Delete/5
		public async Task<IActionResult> Delete(string id)
		{
			if (id == null)
			{
				return NotFound();
			}

			IdentityRole role = await _roleManager.Roles.FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false);
			if (role == null)
			{
				return NotFound();
			}

			return View(new Role()
			{
				RoleId = role.Id,
				Name = role.Name
			});
		}

		// POST: Roles/Roles/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(string id)
		{
			IdentityRole role = await _roleManager.Roles.FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false);
			Task<IdentityResult> roleTask = _roleManager.DeleteAsync(role);
			roleTask.Wait();
			return RedirectToAction(nameof(Index));
		}

		private bool RoleExists(string id)
		{
			return _roleManager.Roles.Any(r => r.Id == id);
		}
	}
}
