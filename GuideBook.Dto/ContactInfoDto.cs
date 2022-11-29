using EnumHelper;

namespace GuideBook.Dto;

public class ContactInfoDto : EntityBaseDto
{
    public EContactType ContactType { get; set; }
    public string Info { get; set; }
    public Guid PersonId { get; set; }
}
