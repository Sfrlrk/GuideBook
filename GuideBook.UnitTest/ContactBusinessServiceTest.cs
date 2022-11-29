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

    public async Task CreateContactInfo_Test()
    {
        var person = new PersonDto()
        {
            Company = "Update Company",
            Name = "Update Name",
            Surname = "Update UserName"
        };
        var addedPerson = await personService.Create(person);
        if (addedPerson.IsSuccess)
        {
            var addInfoUser = await contactInfoService.Create(new ContactInfoDto()
            {
                Info = "123456",
                ContactType = EnumHelper.EContactType.Phone,
                PersonId = addedPerson.Data.Id
            });

            if (addInfoUser.IsSuccess)
            {
                var testResult = addInfoUser.Data.IsDeepEqual(new ContactInfoDto()
                {
                    Id = addInfoUser.Data.Id,
                    Info = "123456",
                    ContactType = EnumHelper.EContactType.Phone,
                    PersonId = addedPerson.Data.Id
                });
                    Assert.True(testResult);
            }
            else
                Assert.True(false, "Contact not added");
        }
        else
            Assert.True(false, "Person not added");
    }

}