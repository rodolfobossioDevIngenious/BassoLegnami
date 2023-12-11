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
	public class RecordFilterRulesController : BassoLegnami.Controllers.BaseController
	{
		private readonly IUnitOfWork _unitOfWork;

		public RecordFilterRulesController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		// GET: Users/RecordFilterRules/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			RecordFilterRule recordFilterRule = await _unitOfWork.RecordFilterRulesRepository.FindAsync(m => m.RecordFilterRuleID == id, r => r.RecordFilterRuleType, r => r.RecordFilterRuleValues).ConfigureAwait(false);
			if (recordFilterRule == null)
			{
				return NotFound();
			}

			return View(recordFilterRule);
		}

		// GET: Users/RecordFilterRules/Create
		public IActionResult Create(string id)
		{
			ViewData["RecordFilterRuleTypeID"] = new SelectList(_unitOfWork.RecordFilterRuleTypesRepository.GetAll().OrderBy(r => r.Name), "RecordFilterRuleTypeID", "Name");
			return View(new RecordFilterRule() { UserId = new Guid(id) });
		}

		// POST: Users/RecordFilterRules/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("RecordFilterRuleID,UserId,RecordFilterRuleTypeID,RecordFilterRuleValues,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,RowVersion")] RecordFilterRule recordFilterRule)
		{
			if (ModelState.IsValid)
			{
				_unitOfWork.RecordFilterRulesRepository.Add(recordFilterRule);
				await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
				return RedirectToAction(nameof(Details), new { id = recordFilterRule.RecordFilterRuleID });
			}
			ViewData["RecordFilterRuleTypeID"] = new SelectList(_unitOfWork.RecordFilterRuleTypesRepository.GetAll().OrderBy(r => r.Name), "RecordFilterRuleTypeID", "Name", recordFilterRule.RecordFilterRuleTypeID);
			return View(recordFilterRule);
		}

		// GET: Users/RecordFilterRules/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			RecordFilterRule recordFilterRule = await _unitOfWork.RecordFilterRulesRepository.FindAsync(m => m.RecordFilterRuleID == id, r => r.RecordFilterRuleType, r => r.RecordFilterRuleValues).ConfigureAwait(false);
			if (recordFilterRule == null)
			{
				return NotFound();
			}

			ViewData["RecordFilterRuleTypeID"] = new SelectList(_unitOfWork.RecordFilterRuleTypesRepository.GetAll().OrderBy(r => r.Name), "RecordFilterRuleTypeID", "Name", recordFilterRule.RecordFilterRuleTypeID);
			return View(recordFilterRule);
		}

		// POST: Users/RecordFilterRules/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("RecordFilterRuleID,UserId,RecordFilterRuleTypeID,RecordFilterRuleValues,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,RowVersion")] RecordFilterRule recordFilterRule)
		{
			if (id != recordFilterRule.RecordFilterRuleID)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_unitOfWork.RecordFilterRulesRepository.Update(recordFilterRule, id);
					await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!RecordFilterRuleExists(recordFilterRule.RecordFilterRuleID))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Details), new { id = recordFilterRule.RecordFilterRuleID });
			}
			ViewData["RecordFilterRuleTypeID"] = new SelectList(_unitOfWork.RecordFilterRuleTypesRepository.GetAll().OrderBy(r => r.Name), "RecordFilterRuleTypeID", "Name", recordFilterRule.RecordFilterRuleTypeID);
			return View(recordFilterRule);
		}

		// GET: Users/RecordFilterRules/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			RecordFilterRule recordFilterRule = await _unitOfWork.RecordFilterRulesRepository.FindAsync(m => m.RecordFilterRuleID == id, r => r.RecordFilterRuleType, r => r.RecordFilterRuleValues).ConfigureAwait(false);
			if (recordFilterRule == null)
			{
				return NotFound();
			}

			return View(recordFilterRule);
		}

		// POST: Users/RecordFilterRules/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			RecordFilterRule recordFilterRule = _unitOfWork.RecordFilterRulesRepository.Get(id);
			Guid userId = recordFilterRule.UserId;
			_unitOfWork.RecordFilterRulesRepository.Delete(recordFilterRule);
			await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
			return RedirectToAction(nameof(Details), "Users", new { id = userId, area = "Users" });
		}

		private bool RecordFilterRuleExists(int id)
		{
			return _unitOfWork.RecordFilterRulesRepository.Any(e => e.RecordFilterRuleID == id);
		}

		public IActionResult AddRecordFilterRuleValue()
		{
			return View("RecordFilterRuleValueEdit", new RecordFilterRuleValue());
		}
	}
}
