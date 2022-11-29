using GuideBook.BLayer.Interfaces;
using GuideBook.Entities;
using GuideBook.Dto;
using GuideBook.Dto.ServiceResults;
using Microsoft.AspNetCore.Mvc;

namespace GuideBook.ReportApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        public ReportController(IReportService reportService) => _reportService = reportService;

        [HttpGet("/RequestReportByLocation/{email_address}/{location}")]
        public async Task<ServiceResult<ReportViewModel>> ReceiveReportByLocation(string email_address, string location) => await _reportService.ReceiveReportByLocation(location, email_address);

        [HttpGet]
        [Route("ChangeReportStatus/{reportId}")]
        public async Task<ServiceResult<bool>> ChangeReportStatus(Guid reportId) => await _reportService.ChangeReportStatus(reportId);

        [HttpGet]
        [Route("/GetAllReports")]
        public async Task<ServiceResult<List<Report>>> GetAllReports() => await _reportService.GetAllReports();
    }
}