using iTextSharp.text;
using iTextSharp.text.pdf;
using System.ComponentModel.DataAnnotations;
using System.Resources;

namespace BassoLegnami.Reports
{
    public class StampaTavoleNormailUfficio : Report
    {
        public const string DATA = "LabelsStampaTavoleNormailUfficioData";
        protected override void PrintBoby()
        {
            float verticalPosition = 0;
            const int MARGIN = 15;
            const int DEFAULT_PARAGRAPH_WIDTH = 180;
            GiacenzeDATA? data = Variables[DATA] as GiacenzeDATA;

            Font standardFont = FontFactory.GetFont(FontFactory.HELVETICA, 10F, Color.BLACK);
            Font secondFont = FontFactory.GetFont(FontFactory.HELVETICA_OBLIQUE, 8F, Color.ORANGE);
            Font obliqueFont = FontFactory.GetFont(FontFactory.HELVETICA_OBLIQUE, 10F, Color.BLACK);
            Font boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10F, Color.BLACK);

            verticalPosition += 50;
            PrintText("Basso Legnami SRL", 105, verticalPosition, 0, Alignment.Center, boldFont); verticalPosition += 20;

            PrintParagraph("Essenza: " + data.Essenza, MARGIN, verticalPosition, DEFAULT_PARAGRAPH_WIDTH, 1, Alignment.Justify, standardFont); verticalPosition += 2;
            PrintParagraph("Stagionatura: " + data.Stagionatura, MARGIN, verticalPosition, DEFAULT_PARAGRAPH_WIDTH, DEFAULT_LINE_HEIGHT * 2, 1, Alignment.Justify, standardFont); verticalPosition += 2;
            PrintParagraph("Stato: " + data.StatoLegno, MARGIN, verticalPosition, DEFAULT_PARAGRAPH_WIDTH, DEFAULT_LINE_HEIGHT * 2, 1, Alignment.Justify, standardFont); verticalPosition += 2;
            PrintParagraph("Classifica: " + data.Classifica, MARGIN, verticalPosition, DEFAULT_PARAGRAPH_WIDTH, DEFAULT_LINE_HEIGHT * 2, 1, Alignment.Justify, standardFont); verticalPosition += 2;

            PdfPTable table = new PdfPTable(2) { SplitLate = true, SplitRows = false, TotalWidth = GetPDFXPos(210 - MARGIN * 2), LockedWidth = true, };
            float[] columnWidths = new float[14] { 5, 10, 10, 5, 10, 5, 5, 10, 5, 5, 10, 10, 5, 5 };
            table.SetWidths(columnWidths);

            PdfPCell cell = new PdfPCell(new Phrase(new Chunk("Spes", DefaultFont))) { BackgroundColor = Color.LIGHT_GRAY }; table.AddCell(cell);
            cell = new PdfPCell(new Phrase(new Chunk("Lunghezza", DefaultFont))) { BackgroundColor = Color.LIGHT_GRAY }; table.AddCell(cell);
            cell = new PdfPCell(new Phrase(new Chunk("Pacco", DefaultFont))) { BackgroundColor = Color.LIGHT_GRAY }; table.AddCell(cell);
            cell = new PdfPCell(new Phrase(new Chunk("Tav", DefaultFont))) { BackgroundColor = Color.LIGHT_GRAY }; table.AddCell(cell);
            cell = new PdfPCell(new Phrase(new Chunk("M Cubi", DefaultFont))) { BackgroundColor = Color.LIGHT_GRAY }; table.AddCell(cell);
            cell = new PdfPCell(new Phrase(new Chunk("Impegn", DefaultFont))) { BackgroundColor = Color.LIGHT_GRAY }; table.AddCell(cell);
            cell = new PdfPCell(new Phrase(new Chunk("Stato", DefaultFont))) { BackgroundColor = Color.LIGHT_GRAY }; table.AddCell(cell);
            cell = new PdfPCell(new Phrase(new Chunk("Classifica", DefaultFont))) { BackgroundColor = Color.LIGHT_GRAY }; table.AddCell(cell);
            cell = new PdfPCell(new Phrase(new Chunk("Stagionatura", DefaultFont))) { BackgroundColor = Color.LIGHT_GRAY }; table.AddCell(cell);
            cell = new PdfPCell(new Phrase(new Chunk("Dep", DefaultFont))) { BackgroundColor = Color.LIGHT_GRAY }; table.AddCell(cell);
            cell = new PdfPCell(new Phrase(new Chunk("Fornitore", DefaultFont))) { BackgroundColor = Color.LIGHT_GRAY }; table.AddCell(cell);
            cell = new PdfPCell(new Phrase(new Chunk("Arrivo", DefaultFont))) { BackgroundColor = Color.LIGHT_GRAY }; table.AddCell(cell);
            cell = new PdfPCell(new Phrase(new Chunk("Fine Ess", DefaultFont))) { BackgroundColor = Color.LIGHT_GRAY }; table.AddCell(cell);
            cell = new PdfPCell(new Phrase(new Chunk("Certif", DefaultFont))) { BackgroundColor = Color.LIGHT_GRAY }; table.AddCell(cell);
            PrintTable(table, MARGIN * 2, verticalPosition); verticalPosition += DEFAULT_LINE_HEIGHT * 18;

        }

        public override string GetModuleName()
        {
            throw new NotImplementedException();
        }

        public class GiacenzeDATA
        {
            public string Essenza { get; set; }
            public string StatoLegno { get; set; }
            public string Stagionatura { get; set; }
            public string Classifica { get; set; }
            public virtual ICollection<GiacenzeDATAList> GiacenzeList { get; set; }

            public GiacenzeDATA()
            {
                GiacenzeList = new HashSet<GiacenzeDATAList>();
            }

            public class GiacenzeDATAList
            {
                public int Id { get; set; }
                public string TipoPacco { get; set; }
                public string Deposito { get; set; }
                public string Provenienza { get; set; }
                public string Fornitore { get; set; }
                public string UnitaMisuraPrezzoAcquisto { get; set; }
                public string Misura { get; set; }
                public string Dim1 { get; set; }
                public string Dim2 { get; set; }
                public string Dim3 { get; set; }
                public int? Quantita { get; set; }
                public string Pacco { get; set; }
                public string Tipo { get; set; }
                public decimal? Volume { get; set; }
                public string NPackList { get; set; }
                public string Marchio { get; set; }
                public string PrezzoAcquisto { get; set; }
                public string QuantitaVenduta { get; set; }
                public string DataImpegno { get; set; }
                public string DataVendita { get; set; }
                public string ClienteImpegno { get; set; }
                public string Certificazione { get; set; }
                public string Qualita { get; set; }
                public string Note { get; set; }
                public string LunghezzaDescr { get; set; }
                public string Strati { get; set; }
                public string NumeroCarico { get; set; }
                public long? RankID { get; set; }
            }
        }
    }
}
