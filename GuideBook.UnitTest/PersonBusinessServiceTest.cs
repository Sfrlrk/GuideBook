using GuideBook.BLayer.Interfaces;
using DeepEqual.Syntax;
using GuideBook.Dto;

namespace GuideBook.PersonTest;

public class PersonBusinessServiceTest
{
    private readonly IPersonService _personService;
    public PersonBusinessServiceTest(IPersonService personService)
    {
        _personService = personService;
    }

    [Fact]
    public async Task CreatePerson_Test()
    {
        var person = new PersonDto()
        {
            Company = "TestCompany",
            Name = "TestName",
            Surname = "TestSurName"
        };
        var resultCreatedPerson = await _personService.Create(person);
        if (resultCreatedPerson.IsSuccess)
        {
            var testResult = resultCreatedPerson.Data.IsDeepEqual(new PersonDto()
            {
                Id = resultCreatedPerson.Data.Id,
                Company = "TestCompany",
                Name = "TestName",
                Surname = "TestSurName"
            });
            Assert.True(testResult);
        }
        else
            Assert.True(false, "An error occured while Create person");
    }

    [Fact]
    public async Task DeletePerson_Test()
    {
        var person = new PersonDto()
        {
            Company = "Delete TestCompany",
            Name = "Delete Test Name",
            Surname = "Delete Test SurName"
        };
        var addedPerson = await _personService.Create(person);
        if (addedPerson.IsSuccess)
        {
            var removePerson = await _personService.Delete(person.Id);
            Assert.True(removePerson.IsSuccess);
        }
        else
            Assert.True(false, "An error occured while add person...");
    }
}