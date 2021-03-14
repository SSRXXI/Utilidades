using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using BitMiracle.Docotic.Pdf;
using Tesseract;

namespace Conversion
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var pdf = new PdfDocument(@"Formulario23.pdf"))
            {
                PdfDrawOptions options = PdfDrawOptions.Create();
                options.BackgroundColor = new PdfRgbColor(255, 255, 255);
                options.HorizontalResolution = 300;
                options.VerticalResolution = 300;

                // save one page
                pdf.Pages[0].Save("Formulario.png", options);
            }
            Bitmap img = new Bitmap("Formulario.png");
            TesseractEngine engine = new TesseractEngine("./tessdata","spa",EngineMode.Default);
            //Page page = engine.Process(img, new Tesseract.Rect(165, 500, 265, 13));
            Page page = engine.Process(img, new Rect(800,1370,800,45),PageSegMode.Auto);
           
            string result = page.GetText();

            int counter = 0;
            string line;

            // Read the file and display it line by line.  
            System.IO.StreamReader file =
                new System.IO.StreamReader(result, Encoding.UTF8);
            while ((line = file.ReadLine()) != null)
            {
                Console.WriteLine(line);
                counter++;
            }

            Console.WriteLine(result);

        }
    }
}
