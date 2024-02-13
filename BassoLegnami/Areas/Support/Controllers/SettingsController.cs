using BassoLegnami.Model.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System;
using BassoLegnami.Model.Models.Support;
using System.Data.SqlClient;
using BassoLegnami.Controllers;
using BassoLegnami.Model.Data;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.ExtendedProperties;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BassoLegnami.Areas.Support.Controllerss
{
    [Area("Support")]
    public class SettingsController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public SettingsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Support/Settings
        public async Task<IActionResult> Index()
        {
            return View(await _unitOfWork.SettingsRepository.GetAllAsync().ConfigureAwait(false));
        }

        // GET: Support/Settings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Setting setting = await _unitOfWork.SettingsRepository.FindAsync(m => m.SettingID == id).ConfigureAwait(false);
            if (setting == null)
            {
                return NotFound();
            }

            return View(setting);
        }

        // GET: Support/Settings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Support/Settings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SettingID,Name,Key,Value,Note,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,RowVersion")] Setting setting)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.SettingsRepository.Add(setting);
                await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
                return RedirectToAction(nameof(Details), new { id = setting.SettingID });
            }
            return View(setting);
        }

        // GET: Support/Settings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Setting setting = await _unitOfWork.SettingsRepository.FindAsync(m => m.SettingID == id).ConfigureAwait(false);
            if (setting == null)
            {
                return NotFound();
            }

            return View(setting);
        }

        // POST: Support/Settings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SettingID,Name,Key,Value,Note,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,RowVersion")] Setting setting)
        {
            if (id != setting.SettingID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.SettingsRepository.Update(setting, id);
                    await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SettingExists(setting.SettingID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), new { id = setting.SettingID });
            }
            return View(setting);
        }

        // GET: Support/Settings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Setting setting = await _unitOfWork.SettingsRepository.FindAsync(m => m.SettingID == id).ConfigureAwait(false);
            if (setting == null)
            {
                return NotFound();
            }

            return View(setting);
        }

        // POST: Support/Settings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Setting setting = _unitOfWork.SettingsRepository.Get(id);
            _unitOfWork.SettingsRepository.Delete(setting);
            await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return RedirectToAction(nameof(Index));
        }

        private bool SettingExists(int id)
        {
            return _unitOfWork.SettingsRepository.Any(e => e.SettingID == id);
        }
    }
}
