using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Commerce.Domain.Interfaces
{
    public interface IRepository<T> : IDisposable where T : class
    {
        void AddUow(T entity);
        void UpdateUow(object id, T entity);
        void RemoveUow(object id);
        
        void InsertOne(T entity);

        void InsertMany(List<T> entities);
    
        IEnumerable<T> Find(Expression<Func<T, bool>> filter);

        List<T> FindAll(int pageIndex, int size);

        List<BsonDocument> Aggregate(BsonDocument[] pipeline);

        T FindOneAndUpdate(FilterDefinition<T> filter, UpdateDefinition<T> update,
            FindOneAndUpdateOptions<T> options = null);

        Task AddAsync(T entity);
        Task AddAsync(IEnumerable<T> entities);
        
        Task UpdateAsync(object id, T entity);
        Task UpdateAsync(IEnumerable<T> entities);
        Task<T> FindByIdAsync(object id);
        Task<IEnumerable<T>> FindAllAsync();
        Task<IEnumerable<T>> FindAllWhereAsync(Expression<Func<T, bool>> where);
        Task<T> FindFirstWhereAsync(Expression<Func<T, bool>> where);

        void FindOneAndDelete(object id);
    }
}