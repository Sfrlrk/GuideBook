using GuideBook.Dto;

namespace GuideBook.BLayer.Interfaces;

public interface IContactInfoService
{
    Task<ServiceResult<ContactInfoDto>> Create(ContactInfoDto dto);
    Task<ServiceResult<ContactInfoDto>> Delete(Guid infoId);
}
