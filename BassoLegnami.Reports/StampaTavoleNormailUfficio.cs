using iTextSharp.text;
using iTextSharp.text.pdf;
using System.ComponentModel.DataAnnotations;
using System.Resources;

namespace BassoLegnami.Reports
{
    public class StampaTavoleNormailUfficio : Report
    {
        public override string ModuleName => nameof(StampaTavoleNormailUfficio);

        protected override void PrintBoby()
        {
            float posY = DEFAULT_LINE_HEIGHT * 3;

            GiacenzeDATA data = (GiacenzeDATA)Variables[DATA];

            PrintParagraph("Basso Legnami SRL", LEFT_MARGIN, DEFAULT_LINE_HEIGHT, DEFAULT_W_POS, DEFAULT_LINE_HEIGHT * 2, 1, Alignment.Justify, TitleFont); posY += DEFAULT_LINE_HEIGHT * 2;

            PrintParagraph("Essenza: " + data.Essenza, LEFT_MARGIN, posY, DEFAULT_W_POS, DEFAULT_LINE_HEIGHT * 2, 1, Alignment.Justify, DefaultFont); posY += DEFAULT_LINE_HEIGHT * 2;
            PrintParagraph("Stagionatura: " + data.Stagionatura, LEFT_MARGIN, posY, DEFAULT_W_POS, DEFAULT_LINE_HEIGHT * 2, 1, Alignment.Justify, DefaultFont); posY += DEFAULT_LINE_HEIGHT * 2;
            PrintParagraph("Stato: " + data.StatoLegno, LEFT_MARGIN, posY, DEFAULT_W_POS, DEFAULT_LINE_HEIGHT * 2, 1, Alignment.Justify, DefaultFont); posY += DEFAULT_LINE_HEIGHT * 2;
            PrintParagraph("Classifica: " + data.Classifica, LEFT_MARGIN, posY, DEFAULT_W_POS, DEFAULT_LINE_HEIGHT * 2, 1, Alignment.Justify, DefaultFont); posY += DEFAULT_LINE_HEIGHT * 2;


            PdfPTable table = new PdfPTable(2) { SplitLate = true, SplitRows = false, TotalWidth = GetPDFXPos(210 - LEFT_MARGIN * 2), LockedWidth = true, };
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
            PrintTable(table, LEFT_MARGIN * 2, posY); posY += DEFAULT_LINE_HEIGHT * 18;

            //PrintParagraph("Descrizione", LEFT_MARGIN, posY, 190, DEFAULT_LINE_HEIGHT * 2, 1, Alignment.Justify, IndexFont); posY += DEFAULT_LINE_HEIGHT * 2;
            //PrintParagraph(data.Description.ToString(), LEFT_MARGIN, posY, 190, DEFAULT_LINE_HEIGHT * 50, 1, Alignment.Justify, StandardFont); posY += DEFAULT_LINE_HEIGHT * 14;
            //AddPage();

            //posY = DEFAULT_LINE_HEIGHT * 3;
            //PrintParagraph("I seguenti aspetti devono essere migliorati?", LEFT_MARGIN, posY, 190, DEFAULT_LINE_HEIGHT * 2, 1, Alignment.Justify, IndexFont); posY += DEFAULT_LINE_HEIGHT * 2;
            //PrintParagraph("Nella tabella sono riportati gli aspetti più rilevanti emersi durante il rilievo.", LEFT_MARGIN, posY, 190, DEFAULT_LINE_HEIGHT * 2, 1, Alignment.Justify, StandardFont); posY += DEFAULT_LINE_HEIGHT;

            //Phrase p = new Phrase(string.Empty, StandardFont)
            //{
            //    new Chunk(LogoKO, 0, 0, true),
            //    new Chunk(" = NECESSARIO UN MIGLIORAMENTO", StandardFont),
            //    new Chunk("   "),
            //    new Chunk(LogoOK, 0, 0, true),
            //    new Chunk(" = NON NECESSARIO UN MIGLIORAMENTO", StandardFont)
            //};
            //PrintParagraph(p, LEFT_MARGIN, posY, 190, DEFAULT_LINE_HEIGHT * 2, 1, Alignment.Justify); posY += DEFAULT_LINE_HEIGHT;
            //table = new PdfPTable(RECAP_COLUMNS) { SplitLate = true, SplitRows = false, TotalWidth = GetPDFXPos(210 - LEFT_MARGIN * 2), LockedWidth = true, };
            //columnWidths = new float[RECAP_COLUMNS];
            //Array.Fill(columnWidths, 100F / RECAP_COLUMNS);
            //table.SetWidths(columnWidths);

            //cell = new PdfPCell(new Phrase(new Chunk("Segnaletica", TableTitleFont))) { BackgroundColor = BaseColor.LightGray }; table.AddCell(cell);
            //cell = new PdfPCell(new Phrase(new Chunk("Accesso sottotetto", TableTitleFont))) { BackgroundColor = BaseColor.LightGray }; table.AddCell(cell);
            //cell = new PdfPCell(new Phrase(new Chunk("Locale macchina", TableTitleFont))) { BackgroundColor = BaseColor.LightGray }; table.AddCell(cell);
            //cell = new PdfPCell(new Phrase(new Chunk("Autoclave", TableTitleFont))) { BackgroundColor = BaseColor.LightGray }; table.AddCell(cell);
            //cell = new PdfPCell(new Phrase(new Chunk(data.IsSignposting ? LogoKO : LogoOK, 0, 0, true))); table.AddCell(cell);
            //cell = new PdfPCell(new Phrase(new Chunk(data.IsAtticAccess ? LogoKO : LogoOK, 0, 0, true))); table.AddCell(cell);
            //cell = new PdfPCell(new Phrase(new Chunk(data.IsMachineLocal ? LogoKO : LogoOK, 0, 0, true))); table.AddCell(cell);
            //cell = new PdfPCell(new Phrase(new Chunk(data.IsAutoclave ? LogoKO : LogoOK, 0, 0, true))); table.AddCell(cell);

            //cell = new PdfPCell(new Phrase(new Chunk("Elementi strutturali", TableTitleFont))) { BackgroundColor = BaseColor.LightGray }; table.AddCell(cell);
            //cell = new PdfPCell(new Phrase(new Chunk("Impianti", TableTitleFont))) { BackgroundColor = BaseColor.LightGray }; table.AddCell(cell);
            //cell = new PdfPCell(new Phrase(new Chunk("Deposito materiali", TableTitleFont))) { BackgroundColor = BaseColor.LightGray }; table.AddCell(cell);
            //cell = new PdfPCell(new Phrase(new Chunk("Accesso copertura", TableTitleFont))) { BackgroundColor = BaseColor.LightGray }; table.AddCell(cell);
            //cell = new PdfPCell(new Phrase(new Chunk(data.IsStructureElement ? LogoKO : LogoOK, 0, 0, true))); table.AddCell(cell);
            //cell = new PdfPCell(new Phrase(new Chunk(data.IsSystem ? LogoKO : LogoOK, 0, 0, true))); table.AddCell(cell);
            //cell = new PdfPCell(new Phrase(new Chunk(data.IsMaterialStorage ? LogoKO : LogoOK, 0, 0, true))); table.AddCell(cell);
            //cell = new PdfPCell(new Phrase(new Chunk(data.IsAccessCoverage ? LogoKO : LogoOK, 0, 0, true))); table.AddCell(cell);

            //cell = new PdfPCell(new Phrase(new Chunk("Infestanti", TableTitleFont))) { BackgroundColor = BaseColor.LightGray }; table.AddCell(cell);
            //cell = new PdfPCell(new Phrase(new Chunk("Materiale sospetto", TableTitleFont))) { BackgroundColor = BaseColor.LightGray }; table.AddCell(cell);
            //cell = new PdfPCell(new Phrase(new Chunk(string.Empty, TableTitleFont))); table.AddCell(cell);
            //cell = new PdfPCell(new Phrase(new Chunk(string.Empty, TableTitleFont))); table.AddCell(cell);
            //cell = new PdfPCell(new Phrase(new Chunk(data.IsWeed ? LogoKO : LogoOK, 0, 0, true))); table.AddCell(cell);
            //cell = new PdfPCell(new Phrase(new Chunk(data.IsSuspectedMaterial ? LogoKO : LogoOK, 0, 0, true))); table.AddCell(cell);
            //cell = new PdfPCell(new Phrase(new Chunk(string.Empty, TableFont))); table.AddCell(cell);
            //cell = new PdfPCell(new Phrase(new Chunk(string.Empty, TableFont))); table.AddCell(cell);
            //PrintTable(table, LEFT_MARGIN * 2, posY); posY += DEFAULT_LINE_HEIGHT * 6;

            //p = new Phrase(string.Empty, StandardFont);
            //if (!string.IsNullOrEmpty(data.Note))
            //{
            //    p.Add(Chunk.Newline);
            //    p.Add(new Chunk("Note", IndexFont));
            //    p.Add(Chunk.Newline);
            //    p.Add(new Chunk(data.Note, StandardFont));
            //    p.Add(Chunk.Newline);
            //    p.Add(Chunk.Newline);
            //}
            //p.Add(Chunk.Newline);
            //p.Add(Chunk.Newline);
            //Image img = Image.GetInstance(GetImagePath("ico_IRarea.jpg")); img.ScaleAbsolute(37, 37);
            //p.Add(new Chunk(img, -5, -14, true));
            //p.Add(new Chunk("IR Area (Indice di Rischio per Area)", IndexFont));
            //p.Add(Chunk.Newline);
            //p.Add(Chunk.Newline);
            //p.Add(Chunk.Newline);
            //p.Add(new Chunk(string.Format(data.IndexRiskDescription, "SOTTOTETTO COMUNE"), StandardFont));
            //PrintParagraph(p, LEFT_MARGIN, posY, 190, DEFAULT_LINE_HEIGHT * 20, 1, Alignment.Justify);

            //if (data.Files.Count != 0)
            //{
            //    AddPage();
            //    posY = DEFAULT_LINE_HEIGHT * 3;
            //    PrintLine(new iTextSharp.text.BaseColor(43, 4, 148), 140, posY, 140, 260, 1);
            //    PrintParagraph("Commenti", 145, posY, 190, 1, Alignment.Justify, IndexFont);
            //    posY = PrintParagraph("File", LEFT_MARGIN, posY, 190, 1, Alignment.Justify, IndexFont) + DEFAULT_LINE_HEIGHT;
            //    int pageCount = 1;
            //    posY = DEFAULT_LINE_HEIGHT * 5;
            //    foreach (string image in data.Files)
            //    {
            //        PrintImage(GetImagePath(image), LEFT_MARGIN, posY, 190, DEFAULT_LINE_HEIGHT * 14f);
            //        for (int i = 0; i < 6; i++)
            //        {
            //            PrintLine(new iTextSharp.text.BaseColor(43, 4, 148), 145, posY + (i + 1) * DEFAULT_LINE_HEIGHT * 2, 200, posY + (i + 1) * DEFAULT_LINE_HEIGHT * 2, 1);
            //        }
            //        if (pageCount % 3 == 0)
            //        {
            //            AddPage();
            //            posY = DEFAULT_LINE_HEIGHT * 4;
            //        }
            //        else
            //        {
            //            posY += DEFAULT_LINE_HEIGHT * 15f;
            //        }

            //        pageCount++;
            //    }
            //}
        }

        public override string GetModuleName()
        {
            throw new NotImplementedException();
        }


        public class GiacenzeDATA
        {
            public int Id { get; set; }
            public string TipoPacco { get; set; }
            public string Essenza { get; set; }
            public string StatoLegno { get; set; }
            public string Stagionatura { get; set; }
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
            public string Classifica { get; set; }
            public long? RankID { get; set; }
        }
    }
}
