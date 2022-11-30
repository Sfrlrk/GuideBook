using GuideBook.Repository.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using Swashbuckle.AspNetCore.Annotations;

namespace GuideBook.Repository;

public abstract class MongoDbEntity : IEntity<Guid>
{
    [SwaggerSchema(ReadOnly = true)]
    [BsonElement(Order = 0)]
    [BsonId(IdGenerator = typeof(GuidGenerator)), BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }
}
