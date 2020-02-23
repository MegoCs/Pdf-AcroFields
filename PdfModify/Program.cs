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
            string pdfTemplate = @"C:\Users\Administrator\Desktop\DownloadedPdfs\Pdf Form\ShipmentDocumentForm.pdf";
            string newFile = @"C:\Users\Administrator\Desktop\DownloadedPdfs\Pdf Form\ShipmentDocumentFormReplaced.pdf";
            PdfReader pdfReader = new PdfReader(pdfTemplate);
            PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(newFile, FileMode.Create));
            AcroFields pdfFormFields = pdfStamper.AcroFields;

            var fontBinary = new WebClient().DownloadData("http://beta.adahi.linkdev.com/Style%20Library/Adahi/fonts/Tahoma.ttf");

            var arialBaseFont = BaseFont.CreateFont("Tahoma.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, false, fontBinary, null);

            pdfFormFields.GenerateAppearances=true;

            pdfFormFields.AddSubstitutionFont(arialBaseFont);

            List<string> filesToWrite = new List<string>();
            foreach (var de in pdfReader.AcroFields.Fields)
            {
                filesToWrite.Add(de.Key);
                pdfFormFields.SetField(de.Key, "احمد مجدي");
            }

            // report by reading values from completed PDF  
            //string sTmp = "W-4 Completed for " + pdfFormFields.GetField("f1_09(0)") + " " + pdfFormFields.GetField("f1_10(0)");
            
            // flatten the form to remove editting options, set it to false  
            // to leave the form open to subsequent manual edits  
            pdfStamper.FormFlattening = false;
            // close the pdf  
            pdfStamper.Close();
        }
    }
}
