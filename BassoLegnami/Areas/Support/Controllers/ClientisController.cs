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
    public class ClientisController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        public ClientisController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index(string partitaIva, string ragioneSociale)
        {
            ViewData["PartitaIva"] = partitaIva;
            ViewData["RagioneSociale"] = ragioneSociale;

            if (!string.IsNullOrEmpty(partitaIva))
            {
                Expression<Func<Clienti, bool>> expression = _ => true;
                if (!string.IsNullOrEmpty(partitaIva) || !string.IsNullOrEmpty(ragioneSociale))
                {
                    expression = expression.And(r => r.PartitaIva == partitaIva);
                }
                if (!string.IsNullOrEmpty(ragioneSociale))
                {
                    expression = expression.And(r => r.RagioneSociale.Contains(ragioneSociale));
                }
                //TODO: Continua con i filtri

                //TODO: qui è necessario chiamare il GetData() passando i filtri che servono per l'esportazione dei record. Gli stessi filtri devono essere utilizzati per la stampa PDF.
                //Problema 1: Performance, non ottimali, molto lento in fase di impaginazione. 
                return View(await _unitOfWork.ClientiRepository
                    .FindBy(expression)
                    .ToListAsync()
                    .ConfigureAwait(false));
            }
            return View(_unitOfWork.ClientiRepository.GetData(null).ToList());
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Clienti clienti = _unitOfWork.ClientiRepository.GetData(id).FirstOrDefault();

            if (clienti == null)
            {
                return NotFound();
            }

            return View(clienti);
        }
    }
}
