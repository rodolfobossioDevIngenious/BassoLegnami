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
using ChoETL;

namespace BassoLegnami.Areas.Users.Controllers
{
	[Area("Users")]
	public class UsersController : BassoLegnami.Controllers.BaseController
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;

		public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			_userManager = userManager;
			_roleManager = roleManager;
		}

		// GET: Users/Users
		public IActionResult Index()
		{
			return View(_userManager.Users.Select(r => new User()
			{
				UserId = r.Id,
				UserName = r.UserName,
				Email = r.Email,
				Enabled = r.Enabled,
				ChangePassword = r.ChangePassword,
			}));
		}

		// GET: Users/Users/Details/5
		public async Task<IActionResult> Details(string id)
		{
			if (id == null)
			{
				return NotFound();
			}

			ApplicationUser user = await _userManager.Users
				.Where(r => r.Id == id)
				.FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false);
			if (user == null)
			{
				return NotFound();
			}

			IList<string> userRoles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
			return View(new User()
			{
				UserId = user.Id,
				UserName = user.UserName,
				Email = user.Email,
				Enabled = user.Enabled,
				ChangePassword = user.ChangePassword,
				Roles = _roleManager.Roles.Where(r => userRoles.Contains(r.Name)).Select(r => new Role
				{
					RoleId = r.Id,
					Name = r.Name
				}).ToList()
			});
		}

		// GET: Users/Users/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Users/Users/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("UserId,UserName,Email,Password,ConfirmPassword,Enabled,ChangePassword")] User user)
		{
			if (ModelState.IsValid)
			{
				IdentityResult createUserTask = await _userManager.CreateAsync(new ApplicationUser
				{
					UserName = user.UserName,
					Email = user.Email,
					Enabled = user.Enabled,
					ChangePassword = user.ChangePassword,
					LastPasswordChangedDate = DateTime.Today,
				}, user.Password).ConfigureAwait(false);
				string userId = (await _userManager.FindByEmailAsync(user.Email).ConfigureAwait(false)).Id;
				return RedirectToAction(nameof(Details), new { id = userId });
			}
			return View(user);
		}

		// GET: Users/Users/Edit/5
		public async Task<IActionResult> Edit(string id)
		{
			if (id == null)
			{
				return NotFound();
			}

			ApplicationUser user = await _userManager.Users.FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false);
			if (user == null)
			{
				return NotFound();
			}

			IList<string> userRoles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
			return View(new User()
			{
				UserId = user.Id,
				UserName = user.UserName,
				Email = user.Email,
				Enabled = user.Enabled,
				ChangePassword = user.ChangePassword,
				Roles = _roleManager.Roles.Where(r => userRoles.Contains(r.Name)).Select(r => new Role
				{
					RoleId = r.Id,
					Name = r.Name
				}).ToList()
			});
		}

		// POST: Users/Users/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(string id, [Bind("UserId,UserName,Email,Password,ConfirmPassword,Enabled,ChangePassword")] User user)
		{
			if (id != user.UserId)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					ApplicationUser t = await _userManager.FindByIdAsync(user.UserId).ConfigureAwait(false);
					t.Email = user.Email;
					t.Enabled = user.Enabled;
					t.ChangePassword = user.ChangePassword;
					t.UserName = user.UserName;
					t.SecurityStamp = Guid.NewGuid().ToString();
					t.LastPasswordChangedDate = DateTime.Today;
					IdentityResult userTask = await _userManager.UpdateAsync(t).ConfigureAwait(false);
					string userToken = await _userManager.GeneratePasswordResetTokenAsync(t).ConfigureAwait(false);
					IdentityResult result = await _userManager.ResetPasswordAsync(t, userToken, user.Password).ConfigureAwait(false);

					if (result.Errors.Any())
					{
						foreach (IdentityError error in result.Errors)
						{
                            ModelState.AddModelError(nameof(user.Password), error.Description);
                        }
                        return View();
					}
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!UserExists(user.UserId))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Details), new { id = user.UserId });
			}
			return View(user);
		}

		// GET: Users/Users/Delete/5
		public async Task<IActionResult> Delete(string id)
		{
			if (id == null)
			{
				return NotFound();
			}

			ApplicationUser user = await _userManager.Users.FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false);
			if (user == null)
			{
				return NotFound();
			}

			IList<string> userRoles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
			return View(new User()
			{
				UserId = user.Id,
				UserName = user.UserName,
				Email = user.Email,
				Enabled = user.Enabled,
				ChangePassword = user.ChangePassword,
				Roles = _roleManager.Roles.Where(r => userRoles.Contains(r.Name)).Select(r => new Role
				{
					RoleId = r.Id,
					Name = r.Name
				}).ToList()
			});
		}

		// POST: Users/Users/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(string id)
		{
			ApplicationUser user = await _userManager.Users.FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false);
			Task<IdentityResult> userTask = _userManager.DeleteAsync(user);
			userTask.Wait();
			return RedirectToAction(nameof(Index));
		}

		private bool UserExists(string id)
		{
			return _userManager.Users.Any(r => r.Id == id);
		}

		public JsonResult AddToRole(string userId, string roleId)
		{
			ApplicationUser user = _userManager.Users.FirstOrDefault(m => m.Id == userId);
			if (user == null)
			{
				return Json(new { Result = false });
			}

			IdentityRole role = _roleManager.Roles.FirstOrDefault(m => m.Id == roleId);
			if (role == null)
			{
				return Json(new { Result = false });
			}

			Task<IdentityResult> userToRoleTask = _userManager.AddToRoleAsync(user, role.Name);
			userToRoleTask.Wait();
			IdentityResult newRole = userToRoleTask.Result;
			return Json(new { Result = newRole.Succeeded });
		}

		public JsonResult RemoveFromRole(string userId, string roleId)
		{
			ApplicationUser user = _userManager.Users.FirstOrDefault(m => m.Id == userId);
			if (user == null)
			{
				return Json(new { Result = false });
			}

			IdentityRole role = _roleManager.Roles.FirstOrDefault(m => m.Id == roleId);
			if (role == null)
			{
				return Json(new { Result = false });
			}

			Task<IdentityResult> userToRoleTask = _userManager.RemoveFromRoleAsync(user, role.Name);
			userToRoleTask.Wait();
			IdentityResult newRole = userToRoleTask.Result;
			return Json(new { Result = newRole.Succeeded });
		}
	}
}
