using GuideBook.BLayer;
using GuideBook.BLayer.Interfaces;
using GuideBook.Dal;
using GuideBook.Dal.Interfaces;
using GuideBook.Helper;
using Microsoft.Extensions.DependencyInjection;

namespace GuideBook.UnitTest;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<MongoDbConnection>(options =>
        {
            options.ConnectionString = "mongodb://localhost:27017";
            options.Database = "GuideBookMongoDB";
        });
        services.AddScoped<IPersonService, PersonService>();
        services.AddScoped<IPersonRepository, PersonRepository>();
        services.AddScoped<IContactInfoService, ContactInfoService>();
        services.AddScoped<IContactInfoRepository, ContactInfoRepository>();
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<IReportRepository, ReportRepository>();
    }
}