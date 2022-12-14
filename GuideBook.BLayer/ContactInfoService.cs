using EnumHelper;
using GuideBook.BLayer.Interfaces;
using GuideBook.Dal.Interfaces;
using GuideBook.Dto;
using GuideBook.Dto.InfoMessages;
using GuideBook.Entities;
using System.Linq.Expressions;

namespace GuideBook.BLayer;

public class ContactInfoService : EntityService<ContactInfo, ContactInfoDto>, IContactInfoService
{
    private IContactInfoRepository Repository => (IContactInfoRepository)repository;
    private readonly IPersonRepository personRepository;
    public ContactInfoService(IContactInfoRepository _contactInfoRepo, IPersonRepository _personRepository) : base(_contactInfoRepo)
    {
        personRepository = _personRepository;
    }

    public override Expression<Func<ContactInfo, ContactInfoDto>> ListMap() => x => new ContactInfoDto
    {
        Id = x.Id,
        ContactType = x.ContactType,
        Info = x.Info,
        PersonId = x.PersonId,
    };

    public override ContactInfoDto ToDTO(ContactInfo bo) => new()
    {
        Id = bo.Id,
        ContactType = bo.ContactType,
        Info = bo.Info,
        PersonId = bo.PersonId
    };
    public override ContactInfo ConvertToBO(ContactInfo bo, ContactInfoDto dto)
    {
        bo.Id = dto.Id;
        bo.ContactType = dto.ContactType;
        bo.Info = dto.Info;
        bo.PersonId = dto.PersonId;
        return bo;
    }

    public async Task<ServiceResult<ContactInfoDto>> Create(ContactInfoDto dto)
    {
        try
        {
            var person = await personRepository.GetByIdAsync(dto.PersonId);
            if (person == null)
            {
                return new ServiceResult<ContactInfoDto>(nameof(Messages.PersonNotFound), Messages.PersonNotFound);
            }

            var result = await CreateAsync(dto);
            return new ServiceResult<ContactInfoDto>(nameof(Messages.Success), Messages.Success, result);
        }
        catch
        {
            return new ServiceResult<ContactInfoDto>(nameof(Messages.AnErrorOccured), Messages.AnErrorOccured);
        }
    }
    public async Task<ServiceResult<ContactInfoDto>> Delete(Guid id)
    {
        try
        {
            var person = await GetByIdAsync(id);
            if (person == null)
            {
                return new ServiceResult<ContactInfoDto>(nameof(Messages.ContactNotFound), Messages.ContactNotFound);
            }

            var result = await base.DeleteAsync(id);
            return new ServiceResult<ContactInfoDto>(nameof(Messages.Success), Messages.Success, result);
        }
        catch (Exception)
        {
            return new ServiceResult<ContactInfoDto>(nameof(Messages.AnErrorOccured), Messages.AnErrorOccured);
        }
    }
    public async Task<ServiceResult<ExcelReportViewModel>> GetReportByLocation(string location)
    {
        try
        {
            var excelReportVm = new ExcelReportViewModel(location);

            var _phoneCount = 0;
            excelReportVm.PersonCount = await repository.Count(x => x.ContactType == EContactType.Location && x.Info == location);

            var allPerson = await Repository.GetAll();
            foreach (var item in allPerson)
            {
                var personDetails = await repository.GetAll(x => x.PersonId == item.Id);
                if (personDetails.Any(x => x.Info == location))
                {
                    _phoneCount += personDetails.Where(x => x.ContactType == EContactType.Phone).Select(x => x.Info).Distinct().Count();
                }
            }
            excelReportVm.PhoneCount = _phoneCount;

            return new ServiceResult<ExcelReportViewModel>(nameof(Messages.Success), Messages.Success, excelReportVm);
        }
        catch
        {
            return new ServiceResult<ExcelReportViewModel>(nameof(Messages.AnErrorOccured), Messages.AnErrorOccured);
        }
    }

}
