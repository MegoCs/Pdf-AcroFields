using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace PdfModify
{
    class UpdatePdf
    {
        public void ManipulatePdf(String src, String dest,IDictionary<string,string> pdfParams)
        {
            PdfReader reader = new PdfReader(src);
            Rectangle pagesize = reader.GetPageSize(1);
            PdfStamper stamper = new PdfStamper(reader, new FileStream(dest, FileMode.Create));
            AcroFields form = stamper.AcroFields;
            byte[] fontBinary;
            using (var client = new WebClient())
                fontBinary = client.DownloadData("http://beta.adahi.linkdev.com/Style%20Library/Adahi/fonts/Tahoma.ttf");
            var arialBaseFont = BaseFont.CreateFont("Tahoma.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, false, fontBinary, null);
            form.GenerateAppearances = true;
            form.AddSubstitutionFont(arialBaseFont);

            foreach (var param in pdfParams)
            {
                form.SetField(param.Key, param.Value);
            }
            
            PdfPTable table = new PdfPTable(2);
            table.AddCell("#");
            table.AddCell("description");
            table.HeaderRows = 1;
            table.SetWidths(new int[] { 1, 15 });
            for (int i = 1; i <= 150; i++)
            {
                table.AddCell(i.ToString());
                table.AddCell("test " + i);
            }
            ColumnText column = new ColumnText(stamper.GetOverContent(1));
            Rectangle rectPage1 = new Rectangle(36, 36, 559, 540);
            column.SetSimpleColumn(rectPage1);
            column.AddElement(table);
            int pagecount = 1;
            Rectangle rectPage2 = new Rectangle(36, 36, 559, 806);
            int status = column.Go();
            while (ColumnText.HasMoreText(status))
            {
                status = TriggerNewPage(stamper, pagesize, column, rectPage2, ++pagecount);
            }
            stamper.FormFlattening = false;
            stamper.Close();
            reader.Close();
        }

        public int TriggerNewPage(PdfStamper stamper, Rectangle pagesize, ColumnText column, Rectangle rect, int pagecount)
        {
            stamper.InsertPage(pagecount, pagesize);
            PdfContentByte canvas = stamper.GetOverContent(pagecount);
            column.Canvas = canvas;
            column.SetSimpleColumn(rect);
            return column.Go();
        }
    }
}
