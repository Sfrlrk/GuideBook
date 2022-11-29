using GuideBook.Repository.Interfaces;

namespace GuideBook.Dto;

public class EntityBaseDto : IEntityBase
{
    public Guid Id { get; set; }
}
