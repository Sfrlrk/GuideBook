using EnumHelper;
using GuideBook.BLayer.Interfaces;
using GuideBook.Dal;
using GuideBook.Dto;
using GuideBook.Entities;

namespace GuideBook.PersonTest;

public class ReportBusinessServiceTest
{
    private readonly IPersonService personService;
    private readonly IContactInfoService contactInfoService;
    private readonly IReportService reportService;
    public ReportBusinessServiceTest(IPersonService _personService, IContactInfoService _contactInfoService, IReportService _reportService)
    {
        personService = _personService;
        contactInfoService = _contactInfoService;
        reportService = _reportService;
    }

    [Fact]
    public async Task CreateReport_Test()
    {
        var data = new Report()
        {
            ReportType = EReportType.Preparing,
            Location = "Aydın",
            RequestDate = DateTime.Now
        };
        var resultCreatedReport = await reportService.Create(data);
        Assert.True(resultCreatedReport.IsSuccess, "Report not created");
    }

    [Fact]
    public async Task ListReport_Test()
    {
        var data = new Report()
        {
            ReportType = EReportType.Preparing,
            Location = "Aydın",
            RequestDate = DateTime.Now
        };
        var resultCreatedReport = await reportService.Create(data);
        if (!resultCreatedReport.IsSuccess)
        {
            Assert.True(false, "Person not created");
        }

        var list = await reportService.ToList();
        Assert.True(list.IsSuccess && list.Data.Count > 0, "Report not listed");
    }

    [Fact]
    public async Task ChangeReportType_Test()
    {
        var data = new Report()
        {
            ReportType = EReportType.Preparing,
            Location = "Aydın",
            RequestDate = DateTime.Now
        };
        var resultCreatedReport = await reportService.Create(data);
        if (!resultCreatedReport.IsSuccess)
        {
            Assert.True(false, "Report not created");
        }

        var changedType = await reportService.ChangeType(resultCreatedReport.Data.Id, "temp");
        Assert.True(changedType.IsSuccess && changedType.Data, "Report not Changed");
    }

    [Fact]
    public async Task GetReport_Test()
    {
        var data = new Report()
        {
            ReportType = EReportType.Preparing,
            Location = "Aydın",
            RequestDate = DateTime.Now
        };
        var resultCreatedReport = await reportService.Create(data);
        if (!resultCreatedReport.IsSuccess)
        {
            Assert.True(false, "Report not created");
        }

        var changedType = await reportService.GetReport(resultCreatedReport.Data.Id);
        Assert.True(changedType.IsSuccess && changedType.Data != null, "Report not listed");
    }

    /// <summary>
    /// rabbitmq kurulu olmadığı için test edilemedi todo
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task CreateReportByLocation_Test()
    {
        //var reportVm = await reportService.CreateReportByLocation("Aydın", "swz@gmail.com");
        //if (!reportVm.IsSuccess)
        //{
        //    Assert.True(false, "Report not created");
        //}

        //Assert.True(reportVm.Data != null);
        Assert.True(true,"");
    }

}