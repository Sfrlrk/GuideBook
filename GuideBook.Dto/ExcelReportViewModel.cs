namespace GuideBook.Dto;

public class ExcelReportViewModel
{
    public ExcelReportViewModel(string location) => Location = location;
    public ExcelReportViewModel() { }
    
    public int PersonCount { get; set; }
    public string Location { get; set; }
    public int PhoneCount { get; set; }
}
