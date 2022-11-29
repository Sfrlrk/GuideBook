using GuideBook.BLayer.Interfaces;
using GuideBook.Dto;
using Microsoft.AspNetCore.Mvc;

namespace GuideBook.PersonApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PersonController : ControllerBase
{
    private readonly IPersonService _personService;
    public PersonController(IPersonService personService) => _personService = personService;

    [HttpGet]
    [Route("GetAllDetailsByPersonId/{personId}")]
    public async Task<ServiceResult<ContactInfoViewModel>> GetAllDetailsByPersonId(Guid personId) => await _personService.GetPersonAllData(personId);

    [HttpGet]
    [Route("Delete/{personId}")]
    public async Task<ServiceResult<PersonDto>> Delete(Guid personId) => await _personService.Delete(personId);

    [HttpPost]
    [Route("Create")]
    public async Task<ServiceResult<PersonDto>> Create([FromBody] PersonDto person) => await _personService.Create(person);

    [HttpGet]
    [Route("GetPersonCountWithLocation/{location}")]
    public async Task<ServiceResult<ExcelReportViewModel>> GetPersonCountWithLocation(string location) => await _personService.GetReportWithLocation(location);

}