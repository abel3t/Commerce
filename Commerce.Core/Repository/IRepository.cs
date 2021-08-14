using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Commerce.Core.Repository
{
    public interface IRepository<T> where T : class
    {
        IMongoCollection<T> GetCollection();

        void InsertOne(T entity);

        void InsertMany(List<T> entities);
    
        IEnumerable<T> Find(Expression<Func<T, bool>> filter);

        List<T> FindAll(int pageIndex, int size);

        List<BsonDocument> Aggregate(BsonDocument[] pipeline);

        T FindOneAndUpdate(FilterDefinition<T> filter, UpdateDefinition<T> update,
            FindOneAndUpdateOptions<T> options = null);

        Task AddAsync(T entity);
        Task AddAsync(IEnumerable<T> entities);
        Task AddAsync(IEnumerable<T> entities, string collectionName);
        Task UpdateAsync(object id, T entity);
        Task UpdateAsync(IEnumerable<T> entities);
        Task<T> FindByIdAsync(object id);
        Task<IEnumerable<T>> FindAllAsync();
        Task<IEnumerable<T>> FindAllWhereAsync(Expression<Func<T, bool>> where);
        Task<T> FindFirstWhereAsync(Expression<Func<T, bool>> where);

        Task RenameCollectionAsync(string oldCollectionName, string newCollectionName);
    }
}