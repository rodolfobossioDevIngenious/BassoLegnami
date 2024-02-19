using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BassoLegnami.Model.Models.Support
{
    public class Tabelle : In.Core.Models.Auditable
    {
        public int Id { get; set; }
        public string TipoTabella { get; set; }
        public string Descrizione { get; set; }
        public string CodiceAssociato { get; set; }
        public decimal ValoreAssociato { get; set; }
        public short Predefinito { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }
    }
}
