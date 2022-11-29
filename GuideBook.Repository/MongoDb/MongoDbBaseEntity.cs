using GuideBook.Repository.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Swashbuckle.AspNetCore.Annotations;

namespace GuideBook.Repository;

public abstract class MongoDbEntity : IEntity<Guid>
{
    [SwaggerSchema(ReadOnly = true)]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonId]
    [BsonElement(Order = 0)]
    public Guid Id { get; set; }
}
