using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace BassoLegnami.Reports
{
    public abstract class Report
    {
        public delegate void OpenDocumentEventHandler();
        public delegate void StartPageEventHandler();
        public delegate void EndPageEventHandler();
        public delegate void CloseDocumentEventHandler();

        public event OpenDocumentEventHandler OnOpenDocument;
        public event StartPageEventHandler OnStartPage;
        public event EndPageEventHandler OnEndPage;
        public event CloseDocumentEventHandler OnCloseDocument;

        /// <summary>
        /// Paragraph alignment
        /// </summary>
        public enum Alignment
        {
            Left = 0,
            Center = 1,
            Right = 2,
            Justify = 3,
        }
        #region Constants
        protected const float DEFAULT_BORDER_WIDTH = 10F;
        protected const float DEFAULT_LINE_WIDTH = 0.4F;
        protected const float DOUBLE_LINE_WIDTH = 0.8F;
        protected const string IMAGE_PATH_KEY = "ImageFilePath";
        #endregion
        #region Properties
        public System.Collections.Specialized.OrderedDictionary Variables { get; private set; }
        protected virtual iTextSharp.text.Rectangle PageSize { get { return iTextSharp.text.PageSize.A4; } }
        protected iTextSharp.text.Document PdfDoc { get; private set; }
        protected iTextSharp.text.pdf.PdfWriter PdfWriter { get; private set; }
        protected iTextSharp.text.Font TitleFont { get; private set; }
        public iTextSharp.text.Font FieldTitleFont { get; private set; }
        public iTextSharp.text.Font DefaultFont { get; private set; }
        #endregion
        #region NestedClasses
        private class PageEventHelper : iTextSharp.text.pdf.PdfPageEventHelper
        {
            Report _report;
            public override void OnOpenDocument(iTextSharp.text.pdf.PdfWriter writer, iTextSharp.text.Document document)
            {
                base.OnOpenDocument(writer, document);
                if (_report.OnOpenDocument != null)
                {
                    _report.OnOpenDocument();
                }
            }
            public override void OnStartPage(iTextSharp.text.pdf.PdfWriter writer, iTextSharp.text.Document document)
            {
                base.OnStartPage(writer, document);
                if (_report.OnStartPage != null)
                {
                    _report.OnStartPage();
                }
            }
            public override void OnEndPage(iTextSharp.text.pdf.PdfWriter writer, iTextSharp.text.Document document)
            {
                base.OnEndPage(writer, document);
                if (_report.OnEndPage != null)
                {
                    _report.OnEndPage();
                }
            }
            public override void OnCloseDocument(iTextSharp.text.pdf.PdfWriter writer, iTextSharp.text.Document document)
            {
                base.OnCloseDocument(writer, document);
                if (_report.OnCloseDocument != null)
                {
                    _report.OnCloseDocument();
                }
            }
            public PageEventHelper(Report report)
            {
                _report = report;   
            }
        }
        #endregion
        #region Methods
        public void PrintReport(Stream outputStream)
        {
            //init PDF print engine
            PdfDoc = new iTextSharp.text.Document(PageSize);
            PdfWriter = iTextSharp.text.pdf.PdfWriter.GetInstance(PdfDoc, outputStream);
            PdfWriter.PageEvent = new PageEventHelper(this);

            //open PDF document
            PdfDoc.SetMargins(0, 17, 28, 28);
            PdfDoc.Open();
            PdfDoc.NewPage();                                           //add new page

            //print body
            PrintBoby();

            //close PDF document
            PdfDoc.Close();
            PdfDoc = null;
            PdfWriter = null;
        }
        //public void PrintReport(System.Web.HttpResponse response)
        //{
        //    //prepare response session
        //    response.Clear();
        //    response.ClearContent();
        //    response.ClearHeaders();
        //    response.Buffer = true;
        //    response.BufferOutput = true;
        //    response.ContentType = "Application/PDF";

        //    //print report on OutputStream
        //    PrintReport(response.OutputStream);
        //}
        protected float GetPDFXPos(float xPos)
        {
            return PdfDoc.LeftMargin + PrintConversionUtils.ConvertFromScaleModeUnitToPoint(xPos, Reports.PDFScaleMode.Millimeters);
        }
        protected float GetPDFYPos(float yPos)
        {
            return PdfDoc.Top - PdfDoc.TopMargin + PdfDoc.BottomMargin + PdfDoc.Bottom - PrintConversionUtils.ConvertFromScaleModeUnitToPoint(yPos, Reports.PDFScaleMode.Millimeters);
        }

        /// <summary>
        /// Convert point unit to another scalemode
        /// </summary>
        /// <param name="point">Input point meaure to convert</param>
        /// <param name="scaleMode">Destination scale mode</param>
        /// <returns>Point converted to new scalemode</returns>
        internal static float ConvertFromPointToScaleModeUnit(float point, PDFScaleMode scaleMode)
        {
            switch (scaleMode)
            {
                case PDFScaleMode.Centimeters:
                    return point * 2.54F / 72F;
                case PDFScaleMode.Millimeters:
                    return point * 25.4F / 72F;
                case PDFScaleMode.Twips:
                    return point * 20;
                default:
                    break;
            } //SWITCH

            return 0;
        }

        /// <summary>
        /// Convert scalemode unit to point
        /// </summary>
        /// <param name="scaleModeUnit">Scale mode unit meaure to convert</param>
        /// <param name="scaleMode">Scale mode input measure</param>
        /// <returns>Scalemode unit converted to point</returns>
        internal static float ConvertFromScaleModeUnitToPoint(float scaleModeUnit, PDFScaleMode scaleMode)
        {
            switch (scaleMode)
            {
                case PDFScaleMode.Centimeters:
                    return scaleModeUnit / 2.54F * 72F;
                case PDFScaleMode.Millimeters:
                    return scaleModeUnit / 25.4F * 72F;
                case PDFScaleMode.Twips:
                    return scaleModeUnit / 20;
                default:
                    break;
            } //SWITCH

            return 0;
        }

        private float GetWriterYPos(float yPos)
        {
            return ConvertFromPointToScaleModeUnit(PdfDoc.Top - PdfDoc.TopMargin + PdfDoc.BottomMargin + PdfDoc.Bottom - yPos, PDFScaleMode.Millimeters);
        }

        /// <summary>
        /// Print a line
        /// </summary>
        /// <param name="color">Line color</param>
        /// <param name="BGR">Indicates if color is in BGR format</param>
        /// <param name="x1">X start position</param>
        /// <param name="y1">Y start position</param>
        /// <param name="x2">X end position</param>
        /// <param name="y2">Y end position</param>
        protected void PrintLine(System.Drawing.Color color, float x1, float y1, float x2, float y2, float lineWidth)
        {
            PdfWriter.DirectContent.SetLineWidth(lineWidth);
            PdfWriter.DirectContent.SetRGBColorStroke(color.R, color.G, color.B);
            PdfWriter.DirectContent.MoveTo(GetPDFXPos(x1), GetPDFYPos(y1));
            PdfWriter.DirectContent.LineTo(GetPDFXPos(x2), GetPDFYPos(y2));
            PdfWriter.DirectContent.Stroke();
        }

        /// <summary>
        /// Print a formatted paragraph
        /// </summary>
        /// <param name="text">Text to print</param>
        /// <param name="x1">X start position</param>
        /// <param name="y1">Y start position</param>
        /// <param name="leading">Interline leading</param>
        /// <param name="alignment">Paragraph alignment</param>
        protected float PrintParagraph(string text, float x1, float y1, float w, float h, float leading, Alignment alignment, iTextSharp.text.Font font)
        {
            return PrintParagraph(new iTextSharp.text.Phrase(text ?? string.Empty, font), x1, y1, w, h, leading, alignment);
        }

        /// <summary>
        /// Print a formatted paragraph
        /// </summary>
        /// <param name="text">Text to print</param>
        /// <param name="x1">X start position</param>
        /// <param name="y1">Y start position</param>
        /// <param name="leading">Interline leading</param>
        /// <param name="alignment">Paragraph alignment</param>
        protected float PrintParagraph(string text, float x1, float y1, float w, float leading, Alignment alignment, iTextSharp.text.Font font)
        {
            return PrintParagraph(new iTextSharp.text.Phrase(text ?? string.Empty, font), x1, y1, w, float.MaxValue, leading, alignment);
        }

        protected float PrintParagraph(iTextSharp.text.Phrase phrase, float x1, float y1, float w, float leading, Alignment alignment)
        {
            return PrintParagraph(phrase, x1, y1, w, float.MaxValue, leading, alignment);
        }

        protected float PrintParagraph(iTextSharp.text.Phrase phrase, float x1, float y1, float w, float h, float leading, Alignment alignment)
        {
            float x2 = x1 + w;
            float y2 = y1 + h;
            PdfWriter.DirectContent.SetCharacterSpacing(ConvertFromScaleModeUnitToPoint(0.02F, PDFScaleMode.Millimeters));
            iTextSharp.text.pdf.ColumnText ct = new(PdfWriter.DirectContent);
            ct.SetSimpleColumn(phrase, GetPDFXPos(x1), GetPDFYPos(y1), GetPDFXPos(x2), GetPDFYPos(y2), leading * (phrase.Font.Size + 1), (int)alignment);

            switch (alignment)
            {
                case Alignment.Left:
                    ct.Alignment = iTextSharp.text.Element.ALIGN_LEFT;
                    break;
                case Alignment.Center:
                    ct.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                    break;
                case Alignment.Right:
                    ct.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
                    break;
                case Alignment.Justify:
                    ct.Alignment = iTextSharp.text.Element.ALIGN_JUSTIFIED;
                    break;
                default:
                    break;
            } //SWITCH
            ct.Go();

            return GetWriterYPos(ct.YLine);
        }


        protected void PrintText(string text, float x1, float y1, int rotation, Alignment alignment, iTextSharp.text.Font font)
        {
            int textAlignment = iTextSharp.text.Element.ALIGN_LEFT;
            switch (alignment)
            {
                case Alignment.Left:
                    textAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    break;
                case Alignment.Center:
                    textAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                    break;
                case Alignment.Right:
                    textAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
                    break;
                case Alignment.Justify:
                    textAlignment = iTextSharp.text.Element.ALIGN_JUSTIFIED;
                    break;
                default:
                    break;
            } //SWITCH
            PdfWriter.DirectContent.SetFontAndSize(font.GetCalculatedBaseFont(false), font.Size);
            PdfWriter.DirectContent.BeginText();
            PdfWriter.DirectContent.ShowTextAligned(textAlignment, text, GetPDFXPos(x1), GetPDFYPos(y1), rotation);
            PdfWriter.DirectContent.EndText();
            PdfWriter.DirectContent.Stroke();
        }

        /// <summary>
        /// Print rectangle
        /// </summary>
        /// <param name="color">Border (or fill) color</param>
        /// <param name="BGR">Indicates if color is in BGR format</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="w">Width</param>
        /// <param name="h">Height</param>
        protected void PrintRectangle(System.Drawing.Color color, float x, float y, float w, float h, bool filled, float lineWidht = DEFAULT_LINE_WIDTH)
        {
            if (filled)
            {
                PdfWriter.DirectContent.SetRGBColorStroke(color.R, color.G, color.B);
                PdfWriter.DirectContent.SetRGBColorFill(color.R, color.G, color.B);
                PdfWriter.DirectContent.Rectangle(GetPDFXPos(x), GetPDFYPos(y + h), PrintConversionUtils.ConvertFromScaleModeUnitToPoint(w, PDFScaleMode.Millimeters), PrintConversionUtils.ConvertFromScaleModeUnitToPoint(h, PDFScaleMode.Millimeters));
                PdfWriter.DirectContent.ClosePathFillStroke();
            }
            else
            {
                PrintLine(color, x, y, x + w, y, lineWidht);
                PrintLine(color, x, y, x, y + h, lineWidht);
                PrintLine(color, x + w, y, x + w, y + h, lineWidht);
                PrintLine(color, x, y + h, x + w, y + h, lineWidht);
            }
        }
        protected float PrintTable(iTextSharp.text.pdf.PdfPTable table, float x, float y, float height)
        {
            if (height != iTextSharp.text.pdf.MultiColumnText.AUTOMATIC)
            {
                height = GetPDFYPos(height);
            }

            iTextSharp.text.pdf.ColumnText ct = new(PdfWriter.DirectContent);
            ct.AddElement(table);

            int status = 0;
            while (iTextSharp.text.pdf.ColumnText.HasMoreText(status))
            {
                ct.SetSimpleColumn(GetPDFXPos(x), GetPDFYPos(y), table.TotalWidth, height, 1 * (8 + 1), iTextSharp.text.Element.ALIGN_LEFT);
                status = ct.Go();
                if (iTextSharp.text.pdf.ColumnText.HasMoreText(status))
                {
                    AddPage();
                }
            }

            return GetWriterYPos(ct.YLine);
        }

        protected float PrintTable(iTextSharp.text.pdf.PdfPTable table, float x, float y)
        {
            return PrintTable(table, x, y, iTextSharp.text.pdf.MultiColumnText.AUTOMATIC);
        }
        protected void PrintImage(string filename, float x, float y, float w, float h)
        {
            iTextSharp.text.Image printImg = iTextSharp.text.Image.GetInstance(filename);
            if (printImg != null)
            {
                printImg.SetAbsolutePosition(GetPDFXPos(x), GetPDFYPos(y) - PrintConversionUtils.ConvertFromScaleModeUnitToPoint(h, PDFScaleMode.Millimeters));
                printImg.ScaleAbsolute(PrintConversionUtils.ConvertFromScaleModeUnitToPoint(w, PDFScaleMode.Millimeters), PrintConversionUtils.ConvertFromScaleModeUnitToPoint(h, PDFScaleMode.Millimeters));
                PdfWriter.DirectContent.AddImage(printImg, true);
            } //IF
            printImg = null;

        }
        protected void PrintImage(Bitmap image, float x, float y, float w, float h)
        {
            iTextSharp.text.Image printImg = iTextSharp.text.Image.GetInstance(image, System.Drawing.Imaging.ImageFormat.Png);
            if (printImg != null)
            {
                printImg.SetAbsolutePosition(GetPDFXPos(x), GetPDFYPos(y) - PrintConversionUtils.ConvertFromScaleModeUnitToPoint(h, PDFScaleMode.Millimeters));
                printImg.ScaleAbsolute(PrintConversionUtils.ConvertFromScaleModeUnitToPoint(w, PDFScaleMode.Millimeters), PrintConversionUtils.ConvertFromScaleModeUnitToPoint(h, PDFScaleMode.Millimeters));
                PdfWriter.DirectContent.AddImage(printImg, true);
            } //IF
            printImg = null;

        }
        protected void PrintBarCode(string code, float x, float y, float w, float h, string altTex = null)
        {
            iTextSharp.text.Image printImg = null;
            iTextSharp.text.pdf.Barcode128 codeEAN = new();
            codeEAN.Size = 7;
            codeEAN.Code = code;
            codeEAN.StartStopText = false;
            if (!String.IsNullOrEmpty(altTex))
            {
                codeEAN.AltText = altTex;
            }

            printImg = codeEAN.CreateImageWithBarcode(PdfWriter.DirectContent, iTextSharp.text.Color.BLACK, iTextSharp.text.Color.BLACK);
            if (printImg != null)
            {
                printImg.SetAbsolutePosition(GetPDFXPos(x), GetPDFYPos(y) - PrintConversionUtils.ConvertFromScaleModeUnitToPoint(h, PDFScaleMode.Millimeters));
                printImg.ScaleAbsolute(PrintConversionUtils.ConvertFromScaleModeUnitToPoint(w, PDFScaleMode.Millimeters), PrintConversionUtils.ConvertFromScaleModeUnitToPoint(h, PDFScaleMode.Millimeters));
                PdfWriter.DirectContent.AddImage(printImg, true);
            }
            printImg = null;
        }
        protected string GetImagePath()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images\");
        }

        protected void AddPage()
        {
            PdfDoc.NewPage();
        }
        #endregion
        #region Abstract Methods
        protected abstract void PrintBoby();
        public abstract string GetModuleName();
        #endregion
        #region Constructors/Destructors
        public Report()
        {
            Variables = new System.Collections.Specialized.OrderedDictionary();
            TitleFont = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLDOBLIQUE, 12F);
            TitleFont.SetColor(0, 0, 0);
            FieldTitleFont = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 8F);
            FieldTitleFont.SetColor(0, 0, 0);
            DefaultFont = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 8F);
            DefaultFont.SetColor(0, 0, 0);
        }
        #endregion
    }
}
