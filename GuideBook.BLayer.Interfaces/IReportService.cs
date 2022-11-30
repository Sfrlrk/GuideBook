using EnumHelper;
using GuideBook.Dto;
using GuideBook.Entities;

namespace GuideBook.BLayer.Interfaces;

public interface IReportService
{
    Task<ServiceResult<Report>> Create(Report report);
    Task<ServiceResult<ReportViewModel>> CreateReportByLocation(string location,string email);
    Task<ServiceResult<List<Report>>> ToList();
    Task<ServiceResult<Report>> GetReport(Guid id);
    Task<ServiceResult<bool>> ChangeType(Guid id, EReportType reportType = EReportType.Completed);
}
