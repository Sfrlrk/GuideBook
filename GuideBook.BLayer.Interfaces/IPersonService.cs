using GuideBook.Dto;

namespace GuideBook.BLayer.Interfaces;

public interface IPersonService
{
    Task<ServiceResult<PersonDto>> Create(PersonDto person);
    Task<ServiceResult<PersonDto>> Delete(Guid id);

    Task<ServiceResult<ContactInfoViewModel>> GetPersonAllData(Guid personId);
    Task<ServiceResult<ExcelReportViewModel>> GetReportWithLocation(string location);
}
