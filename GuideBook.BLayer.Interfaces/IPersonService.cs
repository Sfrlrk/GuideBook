using GuideBook.Dto;
using GuideBook.Entities;

namespace GuideBook.BLayer.Interfaces;

public interface IPersonService : IEntityService<Person, PersonDto>
{
    Task<ServiceResult<PersonDto>> Create(PersonDto person);
    Task<ServiceResult<PersonDto>> Delete(Guid id);

    Task<ServiceResult<ContactInfoViewModel>> GetPersonAllData(Guid personId);
}
