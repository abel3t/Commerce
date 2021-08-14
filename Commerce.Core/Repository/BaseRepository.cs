using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Commerce.Core.Repository
{
  public class BaseRepository<T> : IRepository<T> where T : class
  {
    private readonly IMongoDbContext _context;
    private IMongoCollection<T> Collection { get; }

    public IMongoCollection<T> GetCollection() => Collection;

    public BaseRepository(IMongoDbContext context)
    {
      _context = context;
      var collectionName = (typeof(T).GetCustomAttributes(typeof(BsonDiscriminatorAttribute), true)
        .FirstOrDefault() is BsonDiscriminatorAttribute customCollection)
        ? customCollection.Discriminator
        : typeof(T).Name.ToLowerInvariant();
      Collection = context.Database.GetCollection<T>(collectionName);
    }

    #region Private Methods

    private FilterDefinition<T> Id(object value) => Builders<T>.Filter.Eq(nameof(Id).ToLower(), value);

    private IEnumerable<WriteModel<T>> CreateUpdates(IEnumerable<T> items)
    {
      var updates = new List<WriteModel<T>>();

      foreach (var item in items)
      {
        var id = typeof(T).GetProperty("id")?.GetValue(item);

        if (id == default)
        {
          continue;
        }

        updates.Add(new ReplaceOneModel<T>(Id(id), item));
      }

      return updates;
    }

    #endregion

    public virtual void InsertOne(T entity) => Collection.InsertOne(entity);
    public virtual void InsertMany(List<T> entities) => Collection.InsertMany(entities);

    public IEnumerable<T> Find(Expression<Func<T, bool>> filter) => Collection.Find(filter).ToEnumerable();
    public T FindOneAndUpdate(FilterDefinition<T> filter, UpdateDefinition<T> update, FindOneAndUpdateOptions<T> options = null)
    {
      return Collection.FindOneAndUpdate(filter, update, options);
    }
    
    public List<T> FindAll(int pageIndex, int size) => Collection.Find(Builders<T>.Filter.Empty).ToList();

    public List<BsonDocument> Aggregate(BsonDocument[] pipeline)
    {
      return Collection.Aggregate<BsonDocument> (pipeline).ToList();
    }
    
    

    public async Task AddAsync(T entity) => await Collection.InsertOneAsync(entity);

    public async Task AddAsync(IEnumerable<T> entities) => await Collection.InsertManyAsync(entities);

    public async Task AddAsync(IEnumerable<T> entities, string collectionName)
      => await _context.Database.GetCollection<T>(collectionName).InsertManyAsync(entities);

    public async Task UpdateAsync(object id, T entity)
      => await Collection.ReplaceOneAsync(Id(id), entity, new ReplaceOptions() {IsUpsert = false});

    public async Task UpdateAsync(IEnumerable<T> entities) =>
      await Collection.BulkWriteAsync(CreateUpdates(entities));

    public async Task<T> FindByIdAsync(object id) => await Collection.Find(Id(id)).SingleOrDefaultAsync();

    public async Task<IEnumerable<T>> FindAllAsync() =>
      await Collection.Find(Builders<T>.Filter.Empty).ToListAsync();

    public async Task<IEnumerable<T>> FindAllWhereAsync(Expression<Func<T, bool>> where)
      => await Collection.Find(where).ToListAsync();

    public async Task<T> FindFirstWhereAsync(Expression<Func<T, bool>> where) =>
      await Collection.Find(where).FirstOrDefaultAsync();

    public async Task RenameCollectionAsync(string oldCollectionName, string newCollectionName)
      => await _context.Database.RenameCollectionAsync(oldCollectionName, newCollectionName);
  }
}