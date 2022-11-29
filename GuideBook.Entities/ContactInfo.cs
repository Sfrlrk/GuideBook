using EnumHelper;
using GuideBook.Repository;

namespace GuideBook.Entities;

public class ContactInfo : MongoDbEntity
{
    public EContactType ContactType { get; set; }
    public string Info { get; set; }
    public Guid PersonId { get; set; }
}
