using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BassoLegnami.Reports
{
    public static class PDFUtils
    {
        public static MemoryStream MergePDF(List<MemoryStream> inFiles)
        {
            MemoryStream stream = new MemoryStream();
            Document doc = new Document();
            PdfCopy pdf = new PdfCopy(doc, stream) { CloseStream = false };
            doc.Open();

            PdfReader reader = null;
            PdfImportedPage page = null;

            //fixed typo
            inFiles.ForEach(file =>
            {
                reader = new PdfReader(file.ToArray());

                for (int i = 0; i < reader.NumberOfPages; i++)
                {
                    page = pdf.GetImportedPage(reader, i + 1);
                    pdf.AddPage(page);
                }

                pdf.FreeReader(reader);
                reader.Close();
            });

            doc.Close();
            return new MemoryStream(stream.ToArray());
        }

        public static Font GetFont(string fontName, string filename)
        {
            if (!FontFactory.IsRegistered(fontName))
            {
                FontFactory.Register(Environment.GetEnvironmentVariable("SystemRoot") + "\\fonts\\" + filename);
            }
            return FontFactory.GetFont(fontName, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        }
    }
}
