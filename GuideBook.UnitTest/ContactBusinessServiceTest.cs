using DeepEqual.Syntax;
using GuideBook.BLayer.Interfaces;
using GuideBook.Dto;

namespace GuideBook.PersonTest;

public class ContactBusinessServiceTest
{
    private readonly IPersonService personService;
    private readonly IContactInfoService contactInfoService;
    public ContactBusinessServiceTest(IPersonService _personService, IContactInfoService _contactInfoService)
    {
        personService = _personService;
        contactInfoService = _contactInfoService;
    }

    [Fact]
    public async Task CreateContactInfo_Test()
    {
        var person = new PersonDto()
        {
            Company = "Create Contact Company",
            Name = "Create Contact Name",
            Surname = "Create Contact UserName"
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

        var testResult = addInfoUser.Data.IsDeepEqual(new ContactInfoDto()
        {
            Id = addInfoUser.Data.Id,
            Info = "123456",
            ContactType = EnumHelper.EContactType.Phone,
            PersonId = addedPerson.Data.Id
        });
        Assert.True(testResult);
    }

    [Fact]
    public async Task DeleteContact_Test()
    {
        var person = new PersonDto()
        {
            Company = "Delete Contact Company",
            Name = "Delete Contact Name",
            Surname = "Delete Contact UserName"
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

        var removeContact = await contactInfoService.Delete(addInfoUser.Data.Id);
        Assert.True(removeContact.IsSuccess);
    }
}