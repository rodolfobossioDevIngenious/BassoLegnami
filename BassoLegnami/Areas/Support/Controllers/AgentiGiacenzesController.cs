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

namespace BassoLegnami.Areas.Support.Controllers
{
    [Area("Support")]
    public class AgentiGiacenzesController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        public AgentiGiacenzesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index(string essenza)
        {
            ViewData["Essenza"] = essenza;
            if (!string.IsNullOrEmpty(essenza))
            {
                Expression<Func<AgentiGiacenze, bool>> expression = _ => true;
                if (!string.IsNullOrEmpty(essenza))
                {
                    expression = expression.And(r => r.Essenza == essenza);
                }
                //TODO: Continua con i filtri

                //TODO: qui è necessario chiamare il GetData() passando i filtri che servono per l'esportazione dei record. Gli stessi filtri devono essere utilizzati per la stampa PDF.
                //Problema 1: Performance, non ottimali, molto lento in fase di impaginazione. 
                return View(await _unitOfWork.AgentiGiacenzeRepository
                    .FindBy(expression)
                    .ToListAsync()
                    .ConfigureAwait(false));
            }
            return View(_unitOfWork.AgentiGiacenzeRepository.GetData(null).ToList());
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AgentiGiacenze giacenze = _unitOfWork.AgentiGiacenzeRepository.GetAllData(id).FirstOrDefault();

            if (giacenze == null)
            {
                return NotFound();
            }

            return View(giacenze);
        }

        public JsonResult GetIndexTable()
        {
            return Json(_unitOfWork.AgentiGiacenzeRepository.GetDataRank(null)
                        .Select(r => new { r.Id, r.TipoPacco, r.Essenza, r.Classifica, r.StatoLegno, r.Stagionatura, r.Deposito, r.Quantita, r.Volume, r.RankID }).ToList());
        }
    }
}
