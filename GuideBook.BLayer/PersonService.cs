using EnumHelper;
using GuideBook.Entities;
using GuideBook.Dto;
using GuideBook.Dto.ErrorMessages;
using GuideBook.Dal.Interfaces;
using GuideBook.BLayer.Interfaces;

namespace GuideBook.BLayer;

public class PersonService : EntityService<Person, PersonDto>, IPersonService
{
    private IPersonRepository Repository => (IPersonRepository)repository;
    public PersonService(IPersonRepository _personRepository) : base(_personRepository)
    {
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

}
