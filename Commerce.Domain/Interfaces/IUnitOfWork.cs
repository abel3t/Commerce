using System;
using System.Threading.Tasks;

namespace Commerce.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task<bool> Commit();
    }
}