namespace GuideBook.Dto;

public class ExcelReportViewModel
{
    public ExcelReportViewModel(string location) => Location = location;
    public ExcelReportViewModel() { }
    
    public long PersonCount { get; set; }
    public string Location { get; set; }
    public long PhoneCount { get; set; }
}
