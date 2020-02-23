using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfTest
{
    public class Class1
    {
        string filePath = @"..\..\example.pdf";
        string fileResult = @"Result.pdf";
        DocumentCore dc = DocumentCore.Load(filePath);

        // Find a position to insert text. Before this text: "> in this position".
        ContentRange cr = dc.Content.Find("> in this position").FirstOrDefault();

            // Insert new text.
            if (cr != null)
                cr.Start.Insert("New text!");
            dc.Save(fileResult);
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(filePath) { UseShellExecute = true });
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(fileResult) { UseShellExecute = true });
    }
}
