using DinkToPdf;
using Do_An_Chuyen_Nganh.Models;
using System.Drawing.Imaging;
using ColorMode = DinkToPdf.ColorMode;

namespace Do_An_Chuyen_Nganh.Helpers
{
    public static class PdfHelper
    {
        public static byte[] GeneratePdfReport(List<Statistical> data)
        {
            var converter = new SynchronizedConverter(new PdfTools());

            var htmlContent = "<html><head></head><body>";
            // Add your data to the HTML content here
            htmlContent += "</body></html>";

            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Landscape,
            },
                Objects = {
                new ObjectSettings
                {
                    HtmlContent = htmlContent,
                }
            }
            };

            return converter.Convert(doc);
        }
    }
}
