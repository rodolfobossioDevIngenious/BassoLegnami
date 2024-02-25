using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System;
using BassoLegnami.Model.Models.Support;
using BassoLegnami.Controllers;
using BassoLegnami.Model.Data;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc.Rendering;
using BassoLegnami.Areas.Support.Models;
using ChoETL;

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

            return View();
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

        public JsonResult GetIndexTable(int? IdEssenza, int? IdClassifica, int? IdStatoLegno, int? IdStagionatura, int? IdDeposito, int? IdProvenienza, int? tipoStampa, int? tipoTavola)
        {
            return Json(_unitOfWork.AgentiGiacenzeRepository.GetDataRank(IdEssenza, IdClassifica, IdStatoLegno, IdStagionatura, IdDeposito, IdProvenienza)
                        .Select(r => new { r.Id, r.TipoPacco, r.Essenza, r.Classifica, r.StatoLegno, r.Stagionatura, r.Deposito, r.Quantita, r.Volume, r.RankID }).ToList());
        }

        public FileContentResult Print(int? IdEssenza, int? IdClassifica, int? IdStatoLegno, int? IdStagionatura, int? IdDeposito, int? IdProvenienza, int? tipoStampa, int? tipoTavola)
        {
            byte[] data = _unitOfWork.AgentiGiacenzeRepository.Print(_unitOfWork.AgentiGiacenzeRepository
                        .GetData(IdEssenza, IdClassifica, IdStatoLegno, IdStagionatura, IdDeposito, IdProvenienza).ToList(), tipoStampa, tipoTavola);

            return File(data, "application/octet-stream", "Giacenze.pdf");
        }
    }
}
