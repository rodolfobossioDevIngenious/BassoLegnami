using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BassoLegnami.Model.Models.Support
{
	public class Clienti : In.Core.Models.Auditable
    {
        [Display(ResourceType = typeof(Resources.Models.Support.Clienti.Clienti), Name = "Id", Description = "Id")]
        public int Id { get; set; }
        
        [Display(ResourceType = typeof(Resources.Models.Support.Clienti.Clienti), Name = "Codice", Description = "Codice")]
        public string Codice { get; set; }
        
        [Display(ResourceType = typeof(Resources.Models.Support.Clienti.Clienti), Name = "RagioneSociale", Description = "RagioneSociale")]
        public string RagioneSociale { get; set; }
        
        [Display(ResourceType = typeof(Resources.Models.Support.Clienti.Clienti), Name = "Indirizzo", Description = "Indirizzo")]
        public string Indirizzo { get; set; }
        
        [Display(ResourceType = typeof(Resources.Models.Support.Clienti.Clienti), Name = "CAP", Description = "CAP")]
        public string CAP { get; set; }
        
        [Display(ResourceType = typeof(Resources.Models.Support.Clienti.Clienti), Name = "Citta", Description = "Citta")]
        public string Citta { get; set; }
        
        [Display(ResourceType = typeof(Resources.Models.Support.Clienti.Clienti), Name = "Provincia", Description = "Provincia")]
        public string Provincia { get; set; }

        [Display(ResourceType = typeof(Resources.Models.Support.Clienti.Clienti), Name = "PartitaIva", Description = "PartitaIva")]
        public string PartitaIva { get; set; }

        [Display(ResourceType = typeof(Resources.Models.Support.Clienti.Clienti), Name = "Pagamento", Description = "Pagamento")]
        public string Pagamento { get; set; }

        [Display(ResourceType = typeof(Resources.Models.Support.Clienti.Clienti), Name = "Incarico1", Description = "Incarico1")]
        public string Incarico1 { get; set; }

        [Display(ResourceType = typeof(Resources.Models.Support.Clienti.Clienti), Name = "Telefono1", Description = "Telefono1")]
        public string Telefono1 { get; set; }
        
        [Display(ResourceType = typeof(Resources.Models.Support.Clienti.Clienti), Name = "Telefono2", Description = "Telefono2")]
        public string Telefono2 { get; set; }
        
        [Display(ResourceType = typeof(Resources.Models.Support.Clienti.Clienti), Name = "Fax", Description = "Fax")]
        public string Fax { get; set; }
        
        [Display(ResourceType = typeof(Resources.Models.Support.Clienti.Clienti), Name = "Cellulare", Description = "Cellulare")]
        public string Cellulare { get; set; }
        
        [Display(ResourceType = typeof(Resources.Models.Support.Clienti.Clienti), Name = "Email", Description = "Email")]
        public string Email { get; set; }

        [Display(ResourceType = typeof(Resources.Models.Support.Clienti.Clienti), Name = "id_Agente", Description = "id_Agente")]
        public int id_Agente { get; set; }
        
        [Display(ResourceType = typeof(Resources.Models.Support.Clienti.Clienti), Name = "Categoria", Description = "Categoria")]
        public string Categoria { get; set; }

        [NotMapped]
        public string CompleteName => string.Join(" - ", Codice, RagioneSociale);
        
        [NotMapped]
        public string Phones => string.Join(" - ", Telefono1, Telefono2, Cellulare);
        
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}
