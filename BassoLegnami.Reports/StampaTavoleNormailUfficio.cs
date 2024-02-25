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
            bool createTable = true;
            float maxRowHeight = 0;
            float previousHeight = 0;
            int currentRow = 1;
            const int MARGIN = 15;
            const int DEFAULT_PARAGRAPH_WIDTH = 180;
            GiacenzeDATA? data = Variables[DATA] as GiacenzeDATA;
            int rowCount = data.GiacenzeList.Count;

            Font standardFont = FontFactory.GetFont(FontFactory.HELVETICA, 10F, Color.BLACK);
            Font secondFont = FontFactory.GetFont(FontFactory.HELVETICA_OBLIQUE, 8F, Color.ORANGE);
            Font obliqueFont = FontFactory.GetFont(FontFactory.HELVETICA_OBLIQUE, 10F, Color.BLACK);
            Font boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10F, Color.BLACK);

            verticalPosition += 10;
            PrintText("Basso Legnami SRL", 105, verticalPosition, 0, Alignment.Center, boldFont);

            //PrintParagraph("Essenza: " + data.Essenza, MARGIN, verticalPosition, DEFAULT_PARAGRAPH_WIDTH, 1, Alignment.Justify, standardFont); verticalPosition += 2;
            //PrintParagraph("Stagionatura: " + data.Stagionatura, MARGIN, verticalPosition, DEFAULT_PARAGRAPH_WIDTH, DEFAULT_LINE_HEIGHT * 2, 1, Alignment.Justify, standardFont); verticalPosition += 2;
            //PrintParagraph("Stato: " + data.StatoLegno, MARGIN, verticalPosition, DEFAULT_PARAGRAPH_WIDTH, DEFAULT_LINE_HEIGHT * 2, 1, Alignment.Justify, standardFont); verticalPosition += 2;
            //PrintParagraph("Classifica: " + data.Classifica, MARGIN, verticalPosition, DEFAULT_PARAGRAPH_WIDTH, DEFAULT_LINE_HEIGHT * 2, 1, Alignment.Justify, standardFont); verticalPosition += 2;

            verticalPosition += 15;
            PdfPTable table = null;
            PdfPCell cell;
            if (createTable)
            {
                table = new PdfPTable(14)
                {
                    //Tabella
                    SplitLate = true,
                    SplitRows = false,
                    TotalWidth = GetPDFXPos(100),
                    LockedWidth = true,
                    HeaderRows = 1
                };
                table.SetWidths(new int[14] { 1, 1, 1, 1, 1, 1, 1, 1, 50, 1, 1, 1, 1, 3 });
                table.DefaultCell.BorderWidth = DEFAULT_LINE_WIDTH / 4f;

                //Header
                cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(new iTextSharp.text.Chunk("Spes", boldFont)))
                {
                    BackgroundColor = iTextSharp.text.Color.LIGHT_GRAY,
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    BorderWidth = DEFAULT_LINE_WIDTH / 4F,
                }; table.AddCell(cell);
                cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(new iTextSharp.text.Chunk("Lunghezza", boldFont)))
                {
                    BackgroundColor = iTextSharp.text.Color.LIGHT_GRAY,
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    BorderWidth = DEFAULT_LINE_WIDTH / 4F,
                }; table.AddCell(cell);
                cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(new iTextSharp.text.Chunk("Pacco", boldFont)))
                {
                    BackgroundColor = iTextSharp.text.Color.LIGHT_GRAY,
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    BorderWidth = DEFAULT_LINE_WIDTH / 4F,
                }; table.AddCell(cell);
                cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(new iTextSharp.text.Chunk("Tav", boldFont)))
                {
                    BackgroundColor = iTextSharp.text.Color.LIGHT_GRAY,
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    BorderWidth = DEFAULT_LINE_WIDTH / 4F,
                }; table.AddCell(cell);
                cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(new iTextSharp.text.Chunk("M Cubi", boldFont)))
                {
                    BackgroundColor = iTextSharp.text.Color.LIGHT_GRAY,
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    BorderWidth = DEFAULT_LINE_WIDTH / 4F,
                }; table.AddCell(cell);
                cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(new iTextSharp.text.Chunk("Impegn", boldFont)))
                {
                    BackgroundColor = iTextSharp.text.Color.LIGHT_GRAY,
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    BorderWidth = DEFAULT_LINE_WIDTH / 4F,
                }; table.AddCell(cell);
                cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(new iTextSharp.text.Chunk("Stato", boldFont)))
                {
                    BackgroundColor = iTextSharp.text.Color.LIGHT_GRAY,
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    BorderWidth = DEFAULT_LINE_WIDTH / 4F,
                }; table.AddCell(cell);
                cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(new iTextSharp.text.Chunk("Classifica", boldFont)))
                {
                    BackgroundColor = iTextSharp.text.Color.LIGHT_GRAY,
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    BorderWidth = DEFAULT_LINE_WIDTH / 4F,
                }; table.AddCell(cell);
                cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(new iTextSharp.text.Chunk("Stagionatura", boldFont)))
                {
                    BackgroundColor = iTextSharp.text.Color.LIGHT_GRAY,
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    BorderWidth = DEFAULT_LINE_WIDTH / 4F,
                }; table.AddCell(cell);
                cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(new iTextSharp.text.Chunk("Dep", boldFont)))
                {
                    BackgroundColor = iTextSharp.text.Color.LIGHT_GRAY,
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    BorderWidth = DEFAULT_LINE_WIDTH / 4F,
                }; table.AddCell(cell);
                cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(new iTextSharp.text.Chunk("Fornitore", boldFont)))
                {
                    BackgroundColor = iTextSharp.text.Color.LIGHT_GRAY,
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    BorderWidth = DEFAULT_LINE_WIDTH / 4F,
                }; table.AddCell(cell);
                cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(new iTextSharp.text.Chunk("Arrivo", boldFont)))
                {
                    BackgroundColor = iTextSharp.text.Color.LIGHT_GRAY,
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    BorderWidth = DEFAULT_LINE_WIDTH / 4F,
                }; table.AddCell(cell);
                cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(new iTextSharp.text.Chunk("Fine Ess", boldFont)))
                {
                    BackgroundColor = iTextSharp.text.Color.LIGHT_GRAY,
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    BorderWidth = DEFAULT_LINE_WIDTH / 4F,
                }; table.AddCell(cell);
                cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(new iTextSharp.text.Chunk("Certif", boldFont)))
                {
                    BackgroundColor = iTextSharp.text.Color.LIGHT_GRAY,
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    BorderWidth = DEFAULT_LINE_WIDTH / 4F,
                }; table.AddCell(cell);

                createTable = false;
            }
            bool pageInterrupt;
            bool lastPage;

            //Corpo
            while (currentRow <= rowCount)
            {
                //cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(new iTextSharp.text.Chunk(data.GiacenzeList.ElementAt(currentRow - 1).Description, standardFont)))
                cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(string.Empty, standardFont))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    BorderWidth = DEFAULT_LINE_WIDTH / 4F,
                }; table.AddCell(cell);
                cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(new iTextSharp.text.Chunk(data.GiacenzeList.ElementAt(currentRow - 1)?.LunghezzaDescr ?? string.Empty, standardFont)))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                    BorderWidth = DEFAULT_LINE_WIDTH / 4F,
                }; table.AddCell(cell);
                cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(new iTextSharp.text.Chunk(data.GiacenzeList.ElementAt(currentRow - 1)?.Pacco ?? string.Empty, standardFont)))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    BorderWidth = DEFAULT_LINE_WIDTH / 4F,
                }; table.AddCell(cell);
                cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(string.Empty, standardFont))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    BorderWidth = DEFAULT_LINE_WIDTH / 4F,
                }; table.AddCell(cell);
                cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(string.Empty, standardFont))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    BorderWidth = DEFAULT_LINE_WIDTH / 4F,
                }; table.AddCell(cell);
                cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(new iTextSharp.text.Chunk(data.GiacenzeList.ElementAt(currentRow - 1)?.ClienteImpegno ?? string.Empty, standardFont)))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                    BorderWidth = DEFAULT_LINE_WIDTH / 4F,
                }; table.AddCell(cell);
                cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(new iTextSharp.text.Chunk(data.GiacenzeList.ElementAt(currentRow - 1)?.StatoLegno ?? string.Empty, standardFont)))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                    BorderWidth = DEFAULT_LINE_WIDTH / 4F,
                }; table.AddCell(cell);
                cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(new iTextSharp.text.Chunk(data.GiacenzeList.ElementAt(currentRow - 1)?.Classifica ?? string.Empty, standardFont)))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                    BorderWidth = DEFAULT_LINE_WIDTH / 4F,
                }; table.AddCell(cell);
                cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(new iTextSharp.text.Chunk(data.GiacenzeList.ElementAt(currentRow - 1)?.Stagionatura ?? string.Empty, standardFont)))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                    BorderWidth = DEFAULT_LINE_WIDTH / 4F,
                }; table.AddCell(cell);
                cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(new iTextSharp.text.Chunk(data.GiacenzeList.ElementAt(currentRow - 1)?.Deposito ?? string.Empty, standardFont)))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                    BorderWidth = DEFAULT_LINE_WIDTH / 4F,
                }; table.AddCell(cell);
                cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(new iTextSharp.text.Chunk(data.GiacenzeList.ElementAt(currentRow - 1)?.Fornitore ?? string.Empty, standardFont)))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                    BorderWidth = DEFAULT_LINE_WIDTH / 4F,
                }; table.AddCell(cell);
                cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(new iTextSharp.text.Chunk(data.GiacenzeList.ElementAt(currentRow - 1)?.DataVendita ?? string.Empty, standardFont)))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                    BorderWidth = DEFAULT_LINE_WIDTH / 4F,
                }; table.AddCell(cell);
                cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(string.Empty, standardFont))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                    BorderWidth = DEFAULT_LINE_WIDTH / 4F,
                }; table.AddCell(cell);
                cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(new iTextSharp.text.Chunk(data.GiacenzeList.ElementAt(currentRow - 1)?.Certificazione ?? string.Empty, standardFont)))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                    BorderWidth = DEFAULT_LINE_WIDTH / 4F,
                }; table.AddCell(cell);
                currentRow++;
                verticalPosition = table.CalculateHeights(currentRow == 1);
                if (verticalPosition > 250)
                {
                    AddPage();
                }
            }

            table.CompleteRow();
            table.TotalWidth = 165;
            table.LockedWidth = true;

            verticalPosition = table.CalculateHeights(currentRow == 1);
            maxRowHeight = Math.Max(maxRowHeight, verticalPosition - previousHeight);
            previousHeight = verticalPosition;

            //verticalPosition = 20;
            PrintTable(table, 2, verticalPosition, 10);

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
