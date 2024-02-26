using iTextSharp.text;
using iTextSharp.text.pdf;
using System.ComponentModel.DataAnnotations;
using System.Resources;

namespace BassoLegnami.Reports
{
    public class StampaTavoleNormailUfficio : Report
    {
        public const string DATA = "LabelsStampaTavoleNormailUfficioData";
        protected override Rectangle PageSize => new Rectangle(iTextSharp.text.PageSize.A4.Height, iTextSharp.text.PageSize.A4.Width);
        protected override void PrintBoby()
        {
            Font boldDefaultFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 8F);
            boldDefaultFont.SetColor(0, 0, 0);
            GiacenzeDATA? data = Variables[DATA] as GiacenzeDATA;

            Font standardFont = FontFactory.GetFont(FontFactory.HELVETICA, 9F, Color.BLACK);
            Font boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10F, Color.BLACK);

            PrintParagraph(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), 0, 10, 287, 15, 1f, Alignment.Right, DefaultFont);
            PrintParagraph("BASSO LEGNAMI S.R.L.", 15, 15, 267, 5, 1f, Alignment.Center, TitleFont);

            PdfPCell cell;
            PdfPTable table = new PdfPTable(14)
            {
                SplitLate = true,
                SplitRows = false,
                TotalWidth = GetPDFXPos(282),
                LockedWidth = true,
                HeaderRows = 1,
            };
            table.SetWidths(new float[14] { 5F, 7F, 7.5F, 4F, 5F, 5F, 7.5F, 7.5F, 7.5F, 7.5F, 10F, 7.5F, 5F, 5F });

            //Header
            cell = new PdfPCell(new Phrase(new Chunk("Spes", boldFont)))
            {
                BackgroundColor = Color.LIGHT_GRAY,
                HorizontalAlignment = Element.ALIGN_LEFT,
                BorderWidth = DEFAULT_LINE_WIDTH / 4F,
            }; table.AddCell(cell);
            cell = new PdfPCell(new Phrase(new Chunk("Lunghezza", boldFont)))
            {
                BackgroundColor = Color.LIGHT_GRAY,
                HorizontalAlignment = Element.ALIGN_LEFT,
                BorderWidth = DEFAULT_LINE_WIDTH / 4F,
            }; table.AddCell(cell);
            cell = new PdfPCell(new Phrase(new Chunk("Pacco", boldFont)))
            {
                BackgroundColor = Color.LIGHT_GRAY,
                HorizontalAlignment = Element.ALIGN_LEFT,
                BorderWidth = DEFAULT_LINE_WIDTH / 4F,
            }; table.AddCell(cell);
            cell = new PdfPCell(new Phrase(new Chunk("Tav", boldFont)))
            {
                BackgroundColor = Color.LIGHT_GRAY,
                HorizontalAlignment = Element.ALIGN_LEFT,
                BorderWidth = DEFAULT_LINE_WIDTH / 4F,
            }; table.AddCell(cell);
            cell = new PdfPCell(new Phrase(new Chunk("M Cubi", boldFont)))
            {
                BackgroundColor = Color.LIGHT_GRAY,
                HorizontalAlignment = Element.ALIGN_LEFT,
                BorderWidth = DEFAULT_LINE_WIDTH / 4F,
            }; table.AddCell(cell);
            cell = new PdfPCell(new Phrase(new Chunk("Impegn", boldFont)))
            {
                BackgroundColor = Color.LIGHT_GRAY,
                HorizontalAlignment = Element.ALIGN_LEFT,
                BorderWidth = DEFAULT_LINE_WIDTH / 4F,
            }; table.AddCell(cell);
            cell = new PdfPCell(new Phrase(new Chunk("Stato Legno", boldFont)))
            {
                BackgroundColor = Color.LIGHT_GRAY,
                HorizontalAlignment = Element.ALIGN_LEFT,
                BorderWidth = DEFAULT_LINE_WIDTH / 4F,
            }; table.AddCell(cell);
            cell = new PdfPCell(new Phrase(new Chunk("Classifica", boldFont)))
            {
                BackgroundColor = Color.LIGHT_GRAY,
                HorizontalAlignment = Element.ALIGN_LEFT,
                BorderWidth = DEFAULT_LINE_WIDTH / 4F,
            }; table.AddCell(cell);
            cell = new PdfPCell(new Phrase(new Chunk("Stagionatura", boldFont)))
            {
                BackgroundColor = Color.LIGHT_GRAY,
                HorizontalAlignment = Element.ALIGN_LEFT,
                BorderWidth = DEFAULT_LINE_WIDTH / 4F,
            }; table.AddCell(cell);
            cell = new PdfPCell(new Phrase(new Chunk("Dep", boldFont)))
            {
                BackgroundColor = Color.LIGHT_GRAY,
                HorizontalAlignment = Element.ALIGN_LEFT,
                BorderWidth = DEFAULT_LINE_WIDTH / 4F,
            }; table.AddCell(cell);
            cell = new PdfPCell(new Phrase(new Chunk("Fornitore", boldFont)))
            {
                BackgroundColor = Color.LIGHT_GRAY,
                HorizontalAlignment = Element.ALIGN_LEFT,
                BorderWidth = DEFAULT_LINE_WIDTH / 4F,
            }; table.AddCell(cell);
            cell = new PdfPCell(new Phrase(new Chunk("Arrivo", boldFont)))
            {
                BackgroundColor = Color.LIGHT_GRAY,
                HorizontalAlignment = Element.ALIGN_LEFT,
                BorderWidth = DEFAULT_LINE_WIDTH / 4F,
            }; table.AddCell(cell);
            cell = new PdfPCell(new Phrase(new Chunk("Fine Ess", boldFont)))
            {
                BackgroundColor = Color.LIGHT_GRAY,
                HorizontalAlignment = Element.ALIGN_LEFT,
                BorderWidth = DEFAULT_LINE_WIDTH / 4F,
            }; table.AddCell(cell);
            cell = new PdfPCell(new Phrase(new Chunk("Certif", boldFont)))
            {
                BackgroundColor = Color.LIGHT_GRAY,
                HorizontalAlignment = Element.ALIGN_LEFT,
                BorderWidth = DEFAULT_LINE_WIDTH / 4F,
            }; table.AddCell(cell);
            table.CompleteRow();

            foreach (GiacenzeDATA.GiacenzeDATAList item in data.GiacenzeList)
            {
                cell = new PdfPCell(new Phrase(string.Empty, standardFont))
                {
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    BorderWidth = DEFAULT_LINE_WIDTH / 4F,
                }; table.AddCell(cell);
                cell = new PdfPCell(new Phrase(new Chunk(item?.LunghezzaDescr ?? string.Empty, standardFont)))
                {
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    BorderWidth = DEFAULT_LINE_WIDTH / 4F,
                }; table.AddCell(cell);
                cell = new PdfPCell(new Phrase(new Chunk(item?.Pacco ?? string.Empty, standardFont)))
                {
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    BorderWidth = DEFAULT_LINE_WIDTH / 4F,
                }; table.AddCell(cell);
                cell = new PdfPCell(new Phrase(string.Empty, standardFont))
                {
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    BorderWidth = DEFAULT_LINE_WIDTH / 4F,
                }; table.AddCell(cell);
                cell = new PdfPCell(new Phrase(string.Empty, standardFont))
                {
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    BorderWidth = DEFAULT_LINE_WIDTH / 4F,
                }; table.AddCell(cell);
                cell = new PdfPCell(new Phrase(new Chunk(item?.ClienteImpegno ?? string.Empty, standardFont)))
                {
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    BorderWidth = DEFAULT_LINE_WIDTH / 4F,
                }; table.AddCell(cell);
                cell = new PdfPCell(new Phrase(new Chunk(item?.StatoLegno ?? string.Empty, standardFont)))
                {
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    BorderWidth = DEFAULT_LINE_WIDTH / 4F,
                }; table.AddCell(cell);
                cell = new PdfPCell(new Phrase(new Chunk(item?.Classifica ?? string.Empty, standardFont)))
                {
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    BorderWidth = DEFAULT_LINE_WIDTH / 4F,
                }; table.AddCell(cell);
                cell = new PdfPCell(new Phrase(new Chunk(item?.Stagionatura ?? string.Empty, standardFont)))
                {
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    BorderWidth = DEFAULT_LINE_WIDTH / 4F,
                }; table.AddCell(cell);
                cell = new PdfPCell(new Phrase(new Chunk(item?.Deposito ?? string.Empty, standardFont)))
                {
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    BorderWidth = DEFAULT_LINE_WIDTH / 4F,
                }; table.AddCell(cell);
                cell = new PdfPCell(new Phrase(new Chunk(item?.Fornitore ?? string.Empty, standardFont)))
                {
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    BorderWidth = DEFAULT_LINE_WIDTH / 4F,
                }; table.AddCell(cell);
                cell = new PdfPCell(new Phrase(new Chunk(item?.DataVendita ?? string.Empty, standardFont)))
                {
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    BorderWidth = DEFAULT_LINE_WIDTH / 4F,
                }; table.AddCell(cell);
                cell = new PdfPCell(new Phrase(string.Empty, standardFont))
                {
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    BorderWidth = DEFAULT_LINE_WIDTH / 4F,
                }; table.AddCell(cell);
                cell = new PdfPCell(new Phrase(new Chunk(item?.Certificazione ?? string.Empty, standardFont)))
                {
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    BorderWidth = DEFAULT_LINE_WIDTH / 4F,
                }; table.AddCell(cell);
                table.CompleteRow();
            }
            PrintTable(table, 15, 30);

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
                public string Essenza { get; set; }
                public string StatoLegno { get; set; }
                public string Stagionatura { get; set; }
                public string Classifica { get; set; }
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
