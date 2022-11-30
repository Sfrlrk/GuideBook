using GuideBook.BLayer.Interfaces;
using GuideBook.Dto;
using GuideBook.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GuideBook.ReportApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        public ReportController(IReportService reportService) => _reportService = reportService;

        [HttpGet("/CreateReportByLocation/{location}/{email_address}")]
        public async Task<ServiceResult<ReportViewModel>> CreateReportByLocation(string location, string email_address) => await _reportService.CreateReportByLocation(location, email_address);

        [HttpGet]
        [Route("ChangeType/{reportId}")]
        public async Task<ServiceResult<bool>> ChangeType(Guid reportId) => await _reportService.ChangeType(reportId);

        [HttpGet]
        [Route("/ToList")]
        public async Task<ServiceResult<List<Report>>> ToList() => await _reportService.ToList();
    }
}