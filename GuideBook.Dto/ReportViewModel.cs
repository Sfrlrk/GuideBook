using EnumHelper;

namespace GuideBook.Dto;

public class ReportViewModel
{
    public Guid Id { get; set; }
    public string Location { get; set; }
    public DateTime RequestDate { get; set; }
    public EReportType ReportType { get; set; }
    public string EmailAddress { get; set; }
}
