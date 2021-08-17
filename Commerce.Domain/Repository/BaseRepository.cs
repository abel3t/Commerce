using MongoDB.Driver;
using Commerce.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace Commerce.Domain.Repository
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        protected readonly IMongoContext Context;
        protected IMongoCollection<T> Collection;

        protected BaseRepository(IMongoContext context)
        {
            Context = context;

            Collection = Context.GetCollection<T>(typeof(T).Name);
        }
        
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
        
        public virtual void InsertOne(T entity) => Collection.InsertOne(entity);

        public virtual void InsertMany(List<T> entities) => Collection.InsertMany(entities);
        
        public virtual async Task<T> FindById(Guid id)
        {
            var data = await Collection.FindAsync(Builders<T>.Filter.Eq("_id", id));
            return data.SingleOrDefault();
        }

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

        public virtual void FindOneAndDelete(Guid id)
        {
            Context.AddCommand(() => Collection.DeleteOneAsync(Builders<T>.Filter.Eq("_id", id)));
        }

        public void Dispose()
        {
            Context?.Dispose();
        }
    }
}