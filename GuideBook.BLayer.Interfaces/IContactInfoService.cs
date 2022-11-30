using GuideBook.Dto;
using GuideBook.Entities;

namespace GuideBook.BLayer.Interfaces;

public interface IContactInfoService : IEntityService<ContactInfo, ContactInfoDto>
{
    Task<ServiceResult<ContactInfoDto>> Create(ContactInfoDto dto);
    Task<ServiceResult<ContactInfoDto>> Delete(Guid infoId);
}
