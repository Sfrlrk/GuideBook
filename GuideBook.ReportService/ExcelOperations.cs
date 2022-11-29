using ClosedXML.Excel;
using GuideBook.Dto;
using GuideBook.Dto.ServiceResults;
using Newtonsoft.Json;
using RestSharp;

namespace GuideBook.Helper;

public static class ExcelOperations
{
    public static string CreateExcel(string location)
    {
        using var wb = new XLWorkbook();

        var ws = wb.Worksheets.Add("Reports");

        ws.Cell(1, 1).Value = "Konum :";
        ws.Cell(1, 2).Value = "Konumda kayıtlı kişi sayısı";
        ws.Cell(1, 3).Value = "Konumda kayıtlı telefon sayısı";

        using (var client = new RestClient("https://localhost:44366/api"))
        {
            var request = new RestRequest($"Person/GetPersonCountWithLocation/{location}");
            var response = client.GetAsync(request).Result;

            if (!string.IsNullOrEmpty(response.Content))
            {
                var excelData = JsonConvert.DeserializeObject<ServiceResult<ExcelReportViewModel>>(response.Content);

                ws.Cell(2, 1).Value = excelData?.Data?.Location;
                ws.Cell(2, 2).Value = excelData?.Data?.PersonCount;
                ws.Cell(2, 3).Value = excelData?.Data?.PhoneCount;
            }
        }

        var filePath = $"Temp\\RiseLocationReport-{Guid.NewGuid()}.xlsx";
        wb.SaveAs(filePath);

        return filePath;
    }
}
