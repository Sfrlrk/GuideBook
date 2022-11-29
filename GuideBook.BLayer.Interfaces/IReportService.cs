using GuideBook.Dto;
using GuideBook.Entities;

namespace GuideBook.BLayer.Interfaces;

public interface IReportService
{
    Task<ServiceResult<ReportViewModel>> ReceiveReportByLocation(string location,string email);
    Task<ServiceResult<List<Report>>> GetAllReports();
    Task<ServiceResult<Report>> GetReportDetails(Guid id);
    Task<ServiceResult<bool>> ChangeReportStatus(Guid id);
}
