using Do_An_Chuyen_Nganh.Models;
using OfficeOpenXml;

namespace Do_An_Chuyen_Nganh.Helpers
{
    public static class ExcelHelper
    {
        public static byte[] GenerateExcelReport(List<Statistical> data)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Statistics");
                // Add your data to the worksheet here

                // Save the Excel package to a memory stream
                var stream = new MemoryStream();
                package.SaveAs(stream);

                return stream.ToArray();
            }
        }
    }
}
