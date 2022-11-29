using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using GuideBook.Helper;
using System.Linq.Expressions;
using GuideBook.Repository.Interfaces;

namespace GuideBook.Repository;

public abstract class MongoDbRepositoryBase<T> : IRepository<T> where T : MongoDbEntity, new()
{
    protected readonly IMongoCollection<T> Collection;
    private readonly MongoDbConnection settings;

    protected MongoDbRepositoryBase(IOptions<MongoDbConnection> options)
    {
        settings = options.Value;
        var client = new MongoClient(settings.ConnectionString);
        var db = client.GetDatabase(settings.Database);
        Collection = db.GetCollection<T>(typeof(T).Name.ToLowerInvariant());
    }

    public virtual IQueryable<T> Get(Expression<Func<T, bool>> predicate = null) => predicate == null
            ? Collection.AsQueryable()
            : Collection.AsQueryable().Where(predicate);

    public virtual Task<T> GetAsync(Expression<Func<T, bool>> predicate) => Collection.Find(predicate).FirstOrDefaultAsync();

    public virtual Task<T> GetByIdAsync(Guid id) => Collection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public virtual async Task<T> AddAsync(T entity)
    {
        var options = new InsertOneOptions { BypassDocumentValidation = false };
        await Collection.InsertOneAsync(entity, options);
        return entity;
    }

    public virtual async Task<bool> AddRangeAsync(IEnumerable<T> entities)
    {
        var options = new BulkWriteOptions { IsOrdered = false, BypassDocumentValidation = false };
        return (await Collection.BulkWriteAsync((IEnumerable<WriteModel<T>>)entities, options)).IsAcknowledged;
    }

    public virtual async Task<T> UpdateAsync(Guid id, T entity) => await Collection.FindOneAndReplaceAsync(x => x.Id == id, entity);

    public virtual async Task<T> UpdateAsync(T entity, Expression<Func<T, bool>> predicate) => await Collection.FindOneAndReplaceAsync(predicate, entity);

    public virtual async Task<T> DeleteAsync(T entity) => await Collection.FindOneAndDeleteAsync(x => x.Id == entity.Id);

    public virtual async Task<T> DeleteAsync(Guid id) => await Collection.FindOneAndDeleteAsync(x => x.Id == id);

    public virtual async Task<T> DeleteAsync(Expression<Func<T, bool>> filter) => await Collection.FindOneAndDeleteAsync(filter);

    public virtual async Task<List<T>> GetAll(Expression<Func<T, bool>> filter = null)
    {
        return filter == null ? await Collection.Find(new BsonDocument()).ToListAsync() : await Collection.Find(filter).ToListAsync();
    }
}
