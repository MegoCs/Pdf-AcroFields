using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace PdfModify
{
    class Program
    {
        static void Main(string[] args)
        {
            string pdfTemplate = @"C:\Users\Administrator\Desktop\DownloadedPdfs\Pdf Form\ShipmentDocumentNew.pdf";
            string newFile = @"C:\Users\Administrator\Desktop\DownloadedPdfs\Pdf Form\ShipmentDocumentFormNewReplaced.pdf";
            PdfReader pdfReader = new PdfReader(pdfTemplate);
            PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(newFile, FileMode.Create));
            AcroFields pdfFormFields = pdfStamper.AcroFields;
            byte[] fontBinary;
            using (var client = new WebClient())
                fontBinary = client.DownloadData("file:///C:/Worspace/Adahi/CMS/Linkdev.Adahi.SP.Content/Style%20Library/Adahi/fonts/Tahoma.ttf");

            var arialBaseFont = BaseFont.CreateFont("Tahoma.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, false, fontBinary, null);
            var font = new Font(arialBaseFont, 11, Font.BOLD);
            pdfFormFields.GenerateAppearances = true;

            pdfFormFields.AddSubstitutionFont(arialBaseFont);

            List<string> filesToWrite = new List<string>();
            foreach (var de in pdfReader.AcroFields.Fields)
            {
                filesToWrite.Add(de.Key);
                pdfFormFields.SetField(de.Key, $"{de.Key}");
            }

            PdfPTable table = new PdfPTable(6)
            {
                RunDirection = PdfWriter.RUN_DIRECTION_RTL,
            };
            bool internalShipment = true;
            if (internalShipment)
            {
                table.AddCell(new PdfPCell(new Phrase("جهة الارسالية", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new Phrase("", font));
                table.AddCell(new PdfPCell(new Phrase("المحافظة", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new Phrase("", font));
                table.AddCell(new PdfPCell(new Phrase("المدينة المرسل لها", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new Phrase("", font));
                table.AddCell(new PdfPCell(new Phrase("العنوان", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("", font)) { Colspan = 2 });
                table.AddCell(new PdfPCell(new Phrase("الهاتف", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("", font)) { Colspan = 2, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_CENTER });
                table.SetTotalWidth(new float[] { 115, 115, 115, 60, 130, 100 });
            }
            else { 
            
            
            }

            ColumnText column = new ColumnText(pdfStamper.GetOverContent(1));

            Rectangle rectPage1 = new Rectangle(-30, 600, table.TotalWidth, table.CalculateHeights());
            column.SetSimpleColumn(rectPage1);
            column.AddElement(table);
            column.AddElement(new Chunk("\n"));
            column.AddElement(table);
            column.AddElement(new Chunk("\n"));
            column.AddElement(table);
            column.AddElement(new Chunk("\n"));
            column.AddElement(table);
            column.AddElement(new Chunk("\n"));
            column.Go();

            pdfStamper.FormFlattening = true;
            // close the pdf  
            pdfStamper.Close();
        }
    }
}
