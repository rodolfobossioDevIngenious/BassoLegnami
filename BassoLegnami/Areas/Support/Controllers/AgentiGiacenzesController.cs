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
using Microsoft.AspNetCore.Mvc.Rendering;

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

        public async Task<IActionResult> Index(int? IdEssenza, int? IdClassifica, int? IdStatoLegno, int? IdStagionatura, int? IdDeposito, int? IdProvenienza)
        {
            List<Tabelle> data = (await _unitOfWork.TabelleRepository
                                        .GetAll()
                                        .AsNoTracking()
                                        .Where(r => r.TipoTabella == "ESS" || r.TipoTabella == "CLA" || r.TipoTabella == "SLA" || r.TipoTabella == "STG" || r.TipoTabella == "DEP" || r.TipoTabella == "PRO")
                                        .ToListAsync()
                                )
                                .OrderBy(r => r.Descrizione)
                                .ToList();

            ViewData["IdEssenza"] = new SelectList(data.Where(r => r.TipoTabella == "ESS"), "Id", "Descrizione", IdEssenza);
            ViewData["IdClassifica"] = new SelectList(data.Where(r => r.TipoTabella == "CLA"), "Id", "Descrizione", IdClassifica);
            ViewData["IdStatoLegno"] = new SelectList(data.Where(r => r.TipoTabella == "SLA"), "Id", "Descrizione", IdStatoLegno);
            ViewData["IdStagionatura"] = new SelectList(data.Where(r => r.TipoTabella == "STG"), "Id", "Descrizione", IdStagionatura);
            ViewData["IdDeposito"] = new SelectList(data.Where(r => r.TipoTabella == "DEP"), "Id", "Descrizione", IdDeposito);
            ViewData["IdProvenienza"] = new SelectList(data.Where(r => r.TipoTabella == "PRO"), "Id", "Descrizione", IdProvenienza);

            if (IdEssenza.HasValue || IdClassifica.HasValue || IdStatoLegno.HasValue || IdStagionatura.HasValue)
            {
                Expression<Func<AgentiGiacenze, bool>> expression = _ => true;
                if (IdEssenza.HasValue)
                {
                    expression = expression.And(r => r.Essenza == data.FirstOrDefault(r => r.Id == IdEssenza).Descrizione);
                }
                if (IdClassifica.HasValue)
                {
                    expression = expression.And(r => r.Classifica == data.FirstOrDefault(r => r.Id == IdClassifica).Descrizione);
                }
                if (IdStatoLegno.HasValue)
                {
                    expression = expression.And(r => r.Classifica == data.FirstOrDefault(r => r.Id == IdStatoLegno).Descrizione);
                }
                if (IdStagionatura.HasValue)
                {
                    expression = expression.And(r => r.Classifica == data.FirstOrDefault(r => r.Id == IdStagionatura).Descrizione);
                }
                if (IdDeposito.HasValue)
                {
                    expression = expression.And(r => r.Classifica == data.FirstOrDefault(r => r.Id == IdDeposito).Descrizione);
                }
                if (IdProvenienza.HasValue)
                {
                    expression = expression.And(r => r.Classifica == data.FirstOrDefault(r => r.Id == IdProvenienza).Descrizione);
                }

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

        public async Task<ActionResult> Print(string id)
        {
            if (id == null)
            {
                throw new ArgumentException();
            }

            long[] giacenzeID = id.Split("|").Select(r => Convert.ToInt64(r)).ToArray();
            if (await _unitOfWork.AgentiGiacenzeRepository.FindBy(r => giacenzeID.Contains(r.Id)).CountAsync().ConfigureAwait(false) != giacenzeID.Length)
            {
                throw new ArgumentException();
            }

            // print delivery notes
            List<System.IO.MemoryStream> condominium = new List<System.IO.MemoryStream>();
            giacenzeID.ToList().ForEach(r => condominium.Add(_unitOfWork.AgentiGiacenzeRepository.Print(r)));
            return File(Reports.PDFUtils.MergePDF(condominium), "application/pdf");
        }
    }
}
