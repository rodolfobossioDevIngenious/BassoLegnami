using Microsoft.AspNetCore.Mvc;
using BassoLegnami.Controllers;
using BassoLegnami.Model.Data;
using BassoLegnami.Model.Models.Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BassoLegnami.Areas.Support.Controllers
{
    [Area("Support")]
    public class UnitsOfMeasurementController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public UnitsOfMeasurementController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private void _PopulateDropdownList(UnitOfMeasurement unitOfMeasurement)
        {
        }

        // GET: /Support/UnitOfMeasurements/
        public ActionResult Index()
        {
            return View(_unitOfWork.UnitOfMeasurementRepository.GetAll());
        }

        // GET: /Support/UnitOfMeasurements/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            UnitOfMeasurement unitOfMeasurement = _unitOfWork.UnitOfMeasurementRepository.Get(id);
            if (unitOfMeasurement == null)
            {
                return NotFound();
            }

            return View(unitOfMeasurement);
        }

        // GET: /Support/UnitOfMeasurements/Create
        public ActionResult Create()
        {
            UnitOfMeasurement unitOfMeasurement = new UnitOfMeasurement();
            _PopulateDropdownList(unitOfMeasurement);
            return View(unitOfMeasurement);
        }

        // POST: /Support/UnitOfMeasurements/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UnitOfMeasurement unitOfMeasurement)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.UnitOfMeasurementRepository.Add(unitOfMeasurement);
                _unitOfWork.Save();
                return RedirectToAction("Details", new { id = unitOfMeasurement.UnitOfMeasurementID });
            }

            _PopulateDropdownList(unitOfMeasurement);
            return View(unitOfMeasurement);
        }

        // GET: /Support/UnitOfMeasurements/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            UnitOfMeasurement unitOfMeasurement = _unitOfWork.UnitOfMeasurementRepository.Get(id);
            if (unitOfMeasurement == null)
            {
                return NotFound();
            }

            _PopulateDropdownList(unitOfMeasurement);
            return View(unitOfMeasurement);
        }

        // POST: /Support/UnitOfMeasurements/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UnitOfMeasurement unitOfMeasurement)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.UnitOfMeasurementRepository.Update(unitOfMeasurement, unitOfMeasurement.UnitOfMeasurementID);
                _unitOfWork.Save();
                return RedirectToAction("Details", new { id = unitOfMeasurement.UnitOfMeasurementID });
            }
            _PopulateDropdownList(unitOfMeasurement);
            return View(unitOfMeasurement);
        }

        // GET: /Support/UnitOfMeasurements/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            UnitOfMeasurement unitOfMeasurement = _unitOfWork.UnitOfMeasurementRepository.Get(id);
            if (unitOfMeasurement == null)
            {
                return NotFound();
            }

            return View(unitOfMeasurement);
        }

        // POST: /Support/UnitOfMeasurements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UnitOfMeasurement unitOfMeasurement = _unitOfWork.UnitOfMeasurementRepository.Get(id);
            _unitOfWork.UnitOfMeasurementRepository.Delete(unitOfMeasurement);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }
    }
}
