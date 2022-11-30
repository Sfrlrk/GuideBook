using GuideBook.BLayer.Interfaces;
using GuideBook.Dto;
using Microsoft.AspNetCore.Mvc;

namespace GuideBook.ContactApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ContactController : ControllerBase
{
    private readonly IContactInfoService contactInfoService;
    public ContactController(IContactInfoService _contactInfoService) => contactInfoService = _contactInfoService;


    [HttpPost]
    [Route("Create")]
    public async Task<ServiceResult<ContactInfoDto>> Create([FromBody] ContactInfoDto contactInfo) => await contactInfoService.Create(contactInfo);

    [HttpPost]
    [Route("Delete/{infoId}")]
    public async Task<ServiceResult<ContactInfoDto>> Delete(Guid infoId) => await contactInfoService.Delete(infoId);
   
    [HttpGet]
    [Route("GetReportByLocation/{location}")]
    public async Task<ServiceResult<ExcelReportViewModel>> GetReportByLocation(string location) => await contactInfoService.GetReportByLocation(location);
}