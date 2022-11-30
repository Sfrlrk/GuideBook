using GuideBook.BLayer;
using GuideBook.BLayer.Interfaces;
using GuideBook.Dal;
using GuideBook.Dal.Interfaces;
using GuideBook.Helper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<MongoDbConnection>(options =>
{
    options.ConnectionString = builder.Configuration.GetSection(nameof(MongoDbConnection) + ":" + MongoDbConnection.ConnectionStringValue).Value;
    options.Database = builder.Configuration.GetSection(nameof(MongoDbConnection) + ":" + MongoDbConnection.DatabaseValue).Value;
});
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<IContactInfoService, ContactInfoService>();
builder.Services.AddScoped<IContactInfoRepository, ContactInfoRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
