using GuideBook.Dal.Interfaces;
using GuideBook.Entities;
using GuideBook.Helper;
using GuideBook.Repository;
using Microsoft.Extensions.Options;

namespace GuideBook.Dal;

public class ContactInfoRepository : MongoDbRepositoryBase<ContactInfo>, IContactInfoRepository
{
    public ContactInfoRepository(IOptions<MongoDbConnection> options) : base(options)
    {
    }
}
