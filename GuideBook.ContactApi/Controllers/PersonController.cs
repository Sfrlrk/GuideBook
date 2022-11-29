using GuideBook.BLayer.Interfaces;
using GuideBook.Entities;
using GuideBook.Dto;
using GuideBook.Dto.ServiceResults;
using Microsoft.AspNetCore.Mvc;

namespace GuideBook.ContactApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PersonController : ControllerBase
{
    private readonly IPersonService _personService;
    public PersonController(IPersonService personService)
    {
        _personService = personService;
    }

    [HttpGet]
    [Route("GetAllDetailsByPersonId/{personId}")]
    public async Task<ServiceResult<ContactInfoViewModel>> GetAllDetailsByPersonId(Guid personId)
    {
        return await _personService.GetPersonAllData(personId);
    }

    [HttpGet]
    [Route("RemovePerson/{personId}")]
    public async Task<ServiceResult<Person>> RemovePerson(Guid personId)
    {
        return await _personService.RemovePerson(personId);
    }

    [HttpPost]
    [Route("AddInfo")]
    public async Task<ServiceResult<ContactInfoDto>> AddInfo([FromBody] ContactInfoDto personDetails)
    {
        return await _personService.AddInfo(personDetails);
    }

    [HttpPost]
    [Route("CreatePerson")]
    public async Task<ServiceResult<Person>> CreatePerson([FromBody] Person person)
    {
        return await _personService.CreatePerson(person);
    }

    [HttpPost]
    [Route("RemoveInfo/{infoId}")]
    public async Task<ServiceResult<ContactInfoDto>> DeleteInfo(Guid infoId) => await _personService.RemoveInfo(infoId);


    [HttpGet]
    [Route("GetPersonCountWithLocation/{location}")]
    public async Task<ServiceResult<ExcelReportViewModel>> GetPersonCountWithLocation(string location)
    {
        return await _personService.GetReportWithLocation(location);
    }

}