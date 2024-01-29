using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BassoLegnami.Model.Models.Support
{
	public class AgentiGiacenze : In.Core.Models.Auditable
    {
        [Display(ResourceType = typeof(Resources.Models.Support.AgentiGiacenze.AgentiGiacenze), Name = "Id", Description = "Id")]
        public int Id { get; set; }

        [Display(ResourceType = typeof(Resources.Models.Support.AgentiGiacenze.AgentiGiacenze), Name = "TipoPacco", Description = "TipoPacco")]
        public string TipoPacco { get; set; }

        [Display(ResourceType = typeof(Resources.Models.Support.AgentiGiacenze.AgentiGiacenze), Name = "Essenza", Description = "Essenza")]
        public string Essenza { get; set; }

        [Display(ResourceType = typeof(Resources.Models.Support.AgentiGiacenze.AgentiGiacenze), Name = "StatoLegno", Description = "StatoLegno")]
        public string StatoLegno { get; set; }

        [Display(ResourceType = typeof(Resources.Models.Support.AgentiGiacenze.AgentiGiacenze), Name = "Stagionatura", Description = "Stagionatura")]
        public string Stagionatura { get; set; }

        [Display(ResourceType = typeof(Resources.Models.Support.AgentiGiacenze.AgentiGiacenze), Name = "Deposito", Description = "Deposito")]
        public string Deposito { get; set; }

        [Display(ResourceType = typeof(Resources.Models.Support.AgentiGiacenze.AgentiGiacenze), Name = "Provenienza", Description = "Provenienza")]
        public string Provenienza { get; set; }

        [Display(ResourceType = typeof(Resources.Models.Support.AgentiGiacenze.AgentiGiacenze), Name = "Fornitore", Description = "Fornitore")]
        public string Fornitore { get; set; }

        [Display(ResourceType = typeof(Resources.Models.Support.AgentiGiacenze.AgentiGiacenze), Name = "UnitaMisuraPrezzoAcquisto", Description = "UnitaMisuraPrezzoAcquisto")]
        public string UnitaMisuraPrezzoAcquisto { get; set; }

        [Display(ResourceType = typeof(Resources.Models.Support.AgentiGiacenze.AgentiGiacenze), Name = "Misura", Description = "Misura")]
        public string Misura { get; set; }

        [Display(ResourceType = typeof(Resources.Models.Support.AgentiGiacenze.AgentiGiacenze), Name = "Dim1", Description = "Dim1")]
        public string Dim1 { get; set; }

        [Display(ResourceType = typeof(Resources.Models.Support.AgentiGiacenze.AgentiGiacenze), Name = "Dim2", Description = "Dim2")]
        public string Dim2 { get; set; }

        [Display(ResourceType = typeof(Resources.Models.Support.AgentiGiacenze.AgentiGiacenze), Name = "Dim3", Description = "Dim3")]
        public string Dim3 { get; set; }

        [Display(ResourceType = typeof(Resources.Models.Support.AgentiGiacenze.AgentiGiacenze), Name = "Quantita", Description = "Quantita")]
        public int? Quantita { get; set; }

        [Display(ResourceType = typeof(Resources.Models.Support.AgentiGiacenze.AgentiGiacenze), Name = "Pacco", Description = "Pacco")]
        public string Pacco { get; set; }

        [Display(ResourceType = typeof(Resources.Models.Support.AgentiGiacenze.AgentiGiacenze), Name = "Tipo", Description = "Tipo")]
        public string Tipo { get; set; }

        [Display(ResourceType = typeof(Resources.Models.Support.AgentiGiacenze.AgentiGiacenze), Name = "Volume", Description = "Volume")]
        public decimal? Volume { get; set; }

        [Display(ResourceType = typeof(Resources.Models.Support.AgentiGiacenze.AgentiGiacenze), Name = "NPackList", Description = "NPackList")]
        public string NPackList { get; set; }

        [Display(ResourceType = typeof(Resources.Models.Support.AgentiGiacenze.AgentiGiacenze), Name = "Marchio", Description = "Marchio")]
        public string Marchio { get; set; }

        [Display(ResourceType = typeof(Resources.Models.Support.AgentiGiacenze.AgentiGiacenze), Name = "PrezzoAcquisto", Description = "PrezzoAcquisto")]
        public string PrezzoAcquisto { get; set; }

        [Display(ResourceType = typeof(Resources.Models.Support.AgentiGiacenze.AgentiGiacenze), Name = "QuantitaVenduta", Description = "QuantitaVenduta")]
        public string QuantitaVenduta { get; set; }

        [Display(ResourceType = typeof(Resources.Models.Support.AgentiGiacenze.AgentiGiacenze), Name = "DataImpegno", Description = "DataImpegno")]
        public string DataImpegno { get; set; }

        [Display(ResourceType = typeof(Resources.Models.Support.AgentiGiacenze.AgentiGiacenze), Name = "DataVendita", Description = "DataVendita")]
        public string DataVendita { get; set; }

        [Display(ResourceType = typeof(Resources.Models.Support.AgentiGiacenze.AgentiGiacenze), Name = "ClienteImpegno", Description = "ClienteImpegno")]
        public string ClienteImpegno { get; set; }

        [Display(ResourceType = typeof(Resources.Models.Support.AgentiGiacenze.AgentiGiacenze), Name = "Certificazione", Description = "Certificazione")]
        public string Certificazione { get; set; }

        [Display(ResourceType = typeof(Resources.Models.Support.AgentiGiacenze.AgentiGiacenze), Name = "Qualita", Description = "Qualita")]
        public string Qualita { get; set; }

        [Display(ResourceType = typeof(Resources.Models.Support.AgentiGiacenze.AgentiGiacenze), Name = "Note", Description = "Note")]
        public string Note { get; set; }

        [Display(ResourceType = typeof(Resources.Models.Support.AgentiGiacenze.AgentiGiacenze), Name = "LunghezzaDescr", Description = "LunghezzaDescr")]
        public string LunghezzaDescr { get; set; }

        [Display(ResourceType = typeof(Resources.Models.Support.AgentiGiacenze.AgentiGiacenze), Name = "Strati", Description = "Strati")]
        public string Strati { get; set; }

        [Display(ResourceType = typeof(Resources.Models.Support.AgentiGiacenze.AgentiGiacenze), Name = "NumeroCarico", Description = "NumeroCarico")]
        public string NumeroCarico { get; set; }

        [Display(ResourceType = typeof(Resources.Models.Support.AgentiGiacenze.AgentiGiacenze), Name = "Classifica", Description = "Classifica")]
        public string Classifica { get; set; }
        public long? RankID { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}
