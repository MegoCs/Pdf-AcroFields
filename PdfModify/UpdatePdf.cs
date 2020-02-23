using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.IO;

namespace PdfModify
{
    class UpdatePdf
    {
        public void ManipulatePdf(String src, String dest)
        {
            PdfReader reader = new PdfReader(src);
            Rectangle pagesize = reader.GetPageSize(1);
            PdfStamper stamper = new PdfStamper(reader, new FileStream(dest, FileMode.Create));
            AcroFields form = stamper.AcroFields;
            form.SetField("Name", "Jennifer");
            form.SetField("Company", "iText's next customer");
            form.SetField("Country", "No Man's Land");
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
