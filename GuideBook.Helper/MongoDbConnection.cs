namespace GuideBook.Helper;

public class MongoDbConnection
{
    public string ConnectionString { get; set; }
    public string Database { get; set; }

    public const string ConnectionStringValue = nameof(ConnectionString);
    public const string DatabaseValue = nameof(Database);
}
