using GuideBook.Entities;
using GuideBook.Dto;
using GuideBook.Dto.InfoMessages;
using GuideBook.Dal.Interfaces;
using GuideBook.BLayer.Interfaces;
using System.Linq.Expressions;

namespace GuideBook.BLayer;

public class PersonService : EntityService<Person, PersonDto>, IPersonService
{
    private readonly IContactInfoRepository contactInfoRepository;
    private IPersonRepository Repository => (IPersonRepository)repository;
    public PersonService(IPersonRepository _personRepository, IContactInfoRepository _contactInfoRepository) : base(_personRepository)
    {
        contactInfoRepository = _contactInfoRepository;
    }

    public override PersonDto ToDTO(Person bo) => new()
    {
        Id = bo.Id,
        Name = bo.Name,
        Surname = bo.Surname,
        Company = bo.Company
    };
    public override Person ConvertToBO(Person bo, PersonDto dto)
    {
        bo.Id = dto.Id;
        bo.Name = dto.Name;
        bo.Surname = dto.Surname;
        bo.Company = dto.Company;
        return bo;
    }

    public override Expression<Func<Person, PersonDto>> ListMap() => x => new PersonDto
    {
        Id = x.Id,
        Name = x.Name,
        Surname= x.Surname,
        Company= x.Company,
    };

    public async Task<ServiceResult<PersonDto>> Create(PersonDto person)
    {
        try
        {
            if (person == null)
            {
                return new ServiceResult<PersonDto>(nameof(Messages.PersonNotFound), Messages.PersonNotFound);
            }
            var res = await CreateAsync(person);

            return new ServiceResult<PersonDto>(nameof(Messages.Success), Messages.Success, res);
        }
        catch
        {
            return new ServiceResult<PersonDto>(nameof(Messages.AnErrorOccured), Messages.AnErrorOccured);
        }
    }
    public async Task<ServiceResult<PersonDto>> Delete(Guid id)
    {
        try
        {
            var data = await DeleteAsync(id);
            return new ServiceResult<PersonDto>(nameof(Messages.Success), Messages.Success, data);
        }
        catch
        {
            return new ServiceResult<PersonDto>(nameof(Messages.AnErrorOccured), Messages.AnErrorOccured);
        }
    }

    public async Task<ServiceResult<ContactInfoViewModel>> GetPersonAllData(Guid personId)
    {
        try
        {
            var person = await GetByIdAsync(personId);
            if (person == null)
            {
                return new ServiceResult<ContactInfoViewModel>(nameof(Messages.PersonNotFound), Messages.PersonNotFound);
            }

            var contactInfovm = new ContactInfoViewModel
            {
                Person = new PersonDto
                {
                    Id = person.Id,
                    Name = person.Name,
                    Surname = person.Surname,
                    Company = person.Company
                },
                ContactInfos = new List<ContactInfoDto>()
            };

            var personDetails = await contactInfoRepository.GetAll(x => x.PersonId == personId);
            personDetails.ForEach(x =>
            {
                contactInfovm.ContactInfos.Add(new ContactInfoDto
                {
                    ContactType = x.ContactType,
                    Info = x.Info,
                    Id = x.Id
                });
            });
            return new ServiceResult<ContactInfoViewModel>(nameof(Messages.Success), Messages.Success, contactInfovm);
        }
        catch
        {
            return new ServiceResult<ContactInfoViewModel>(nameof(Messages.AnErrorOccured), Messages.AnErrorOccured);
        }
    }
}
