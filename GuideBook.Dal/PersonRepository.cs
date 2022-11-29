using GuideBook.Dal.Interfaces;
using GuideBook.Entities;
using GuideBook.Helper;
using GuideBook.Repository;
using Microsoft.Extensions.Options;

namespace GuideBook.Dal;

public class PersonRepository : MongoDbRepositoryBase<Person>, IPersonRepository
{
    public PersonRepository(IOptions<MongoDbConnection> options) : base(options)
    {
    }
}
