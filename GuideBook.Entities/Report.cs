using EnumHelper;
using GuideBook.Repository;

namespace GuideBook.Entities;

public class Report : MongoDbEntity
{
    public string Location { get; set; }
    public DateTime RequestDate { get; set; }
    public EReportType ReportType { get; set; }
    public string FilePath { get; set; }
}
