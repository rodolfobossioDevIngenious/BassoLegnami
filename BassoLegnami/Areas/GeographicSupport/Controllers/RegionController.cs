using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using BassoLegnami.Model.Models.GeographicSupport;
using BassoLegnami.Controllers;
using BassoLegnami.Model.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;

namespace BassoLegnami.Areas.GeographicSupport.Controllers
{
	[Area("GeographicSupport")]
	public class RegionController : BaseController
	{
		private IUnitOfWork _unitOfWork;
		public RegionController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		private void _PopulateDropdownList(Region region)
		{
			ViewBag.RegionalZoneID = new SelectList(_unitOfWork.RegionRepository.GetAll().OrderBy(r => r.Name), "RegionalZoneID", "Name", region.RegionalZoneID);
		}

		// GET: /GeographicSupport/Region/
		public IActionResult Index()
		{
			return View(_unitOfWork.RegionRepository.GetAllIncluding(r => r.RegionalZone));
		}

		// GET: /GeographicSupport/Region/Details/5
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			Region region = _unitOfWork.RegionRepository.Find(r => r.RegionID == id);
			if (region == null)
			{
				return NotFound();
			}

			return View(region);
		}

		// GET: /GeographicSupport/Region/Create
		public ActionResult Create()
		{
			Region region = new();
			_PopulateDropdownList(region);
			return View(region);
		}

		// POST: /GeographicSupport/Region/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind("RegionID,Name,ISTATCode,RegionalZoneID,Flag1,Timestamp")] Region region)
		{
			if (ModelState.IsValid)
			{
				_unitOfWork.RegionRepository.Add(region);
				_unitOfWork.Save();
				return RedirectToAction(nameof(Index));
			}

			_PopulateDropdownList(region);
			return View(region);
		}

		// GET: /GeographicSupport/Region/Edit/5
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			Region region = _unitOfWork.RegionRepository.Find(r => r.RegionID == id);
			if (region == null)
			{
				return NotFound();
			}

			_PopulateDropdownList(region);
			return View(region);
		}

		// POST: /GeographicSupport/Region/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind("RegionID,Name,ISTATCode,RegionalZoneID,Flag1,Timestamp")] Region region)
		{
			if (ModelState.IsValid)
			{
				_unitOfWork.RegionRepository.Update(region, region.RegionID);
				_unitOfWork.Save();
				return RedirectToAction(nameof(Index));
			}
			_PopulateDropdownList(region);
			return View(region);
		}

		// GET: /GeographicSupport/Region/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			Region region = _unitOfWork.RegionRepository.Find(r => r.RegionID == id);
			if (region == null)
			{
				return NotFound();
			}

			return View(region);
		}

		// POST: /GeographicSupport/Region/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			Region region = _unitOfWork.RegionRepository.Find(r => r.RegionID == id);
			_unitOfWork.RegionRepository.Delete(region);
			_unitOfWork.Save();
			return RedirectToAction(nameof(Index));
		}
	}
}
