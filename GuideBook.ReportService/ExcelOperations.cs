using ClosedXML.Excel;
using GuideBook.Dto;

namespace GuideBook.Helper;

public static class ExcelOperations
{
    public static string CreateExcel(ExcelReportViewModel reportData)
    {
        using var wb = new XLWorkbook();

        var ws = wb.Worksheets.Add("Reports");

        ws.Cell(1, 1).Value = "Konum :";
        ws.Cell(1, 2).Value = "Konumda kayıtlı kişi sayısı";
        ws.Cell(1, 3).Value = "Konumda kayıtlı telefon sayısı";

        if (reportData != null)
        {
            ws.Cell(2, 1).Value = reportData.Location;
            ws.Cell(2, 2).Value = reportData.PersonCount;
            ws.Cell(2, 3).Value = reportData.PhoneCount;
        }
        var filePath = $"Temp\\RiseLocationReport-{Guid.NewGuid()}.xlsx";
        wb.SaveAs(filePath);

        return filePath;
    }
}
