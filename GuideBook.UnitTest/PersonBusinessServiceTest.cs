using GuideBook.BLayer.Interfaces;
using DeepEqual.Syntax;
using GuideBook.Dto;
using GuideBook.BLayer;
using GuideBook.Dto.InfoMessages;

namespace GuideBook.PersonTest;

public class PersonBusinessServiceTest
{
    private readonly IPersonService personService;
    private readonly IContactInfoService contactInfoService;
    public PersonBusinessServiceTest(IPersonService _personService, IContactInfoService _contactInfoService)
    {
        personService = _personService;
        contactInfoService = _contactInfoService;
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
        var resultCreatedPerson = await personService.Create(person);
        if (!resultCreatedPerson.IsSuccess)
        {
            Assert.True(false, "Person not created");
        }

        var testResult = resultCreatedPerson.Data.IsDeepEqual(new PersonDto()
        {
            Id = resultCreatedPerson.Data.Id,
            Company = "TestCompany",
            Name = "TestName",
            Surname = "TestSurName"
        });
        Assert.True(testResult);
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
        var addedPerson = await personService.Create(person);
        if (!addedPerson.IsSuccess)
        {
            Assert.True(false, "Person not created");
        }
        var removePerson = await personService.Delete(person.Id);
        Assert.True(removePerson.IsSuccess);
    }

    [Fact]
    public async Task ListPersons_Test()
    {
        var person = new PersonDto()
        {
            Company = "TestCompany",
            Name = "TestName",
            Surname = "TestSurName"
        };
        var resultCreatedPerson1 = await personService.Create(person);
        if (!resultCreatedPerson1.IsSuccess)
        {
            Assert.True(false, "Person not created");
        }

        var list = await personService.ToListAsync();
        Assert.True(list.Count > 0, "Person not listed");
    }

    [Fact]
    public async Task GetPersonAllData_Test()
    {
        var person = new PersonDto()
        {
            Company = "AllData Company",
            Name = "AllData Name",
            Surname = "AllData UserName"
        };
        var addedPerson = await personService.Create(person);
        if (!addedPerson.IsSuccess)
        {
            Assert.True(false, "Person not added");
        }

        var addInfoUser = await contactInfoService.Create(new ContactInfoDto()
        {
            Info = "123456",
            ContactType = EnumHelper.EContactType.Phone,
            PersonId = addedPerson.Data.Id
        });

        if (!addInfoUser.IsSuccess)
        {
            Assert.True(false, "Contact not added");
        }

        var persons = await personService.GetPersonAllData(addedPerson.Data.Id);

        Assert.True(persons.Data?.Person != null, Messages.PersonNotFound);
        Assert.True(persons.Data.ContactInfos.Count > 0, Messages.ContactNotFound);
    }
}