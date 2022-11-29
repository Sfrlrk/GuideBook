using Microsoft.Extensions.Options;
using GuideBook.Repository;
using GuideBook.Entities;
using GuideBook.Helper;
using GuideBook.Dal.Interfaces;

namespace GuideBook.Dal;

public class ReportRepository : MongoDbRepositoryBase<Report>, IReportRepository
{
    public ReportRepository(IOptions<MongoDbConnection> options) : base(options)
    {
    }
}
