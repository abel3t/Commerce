using Commerce.Domain.Interfaces;
using Commerce.Domain.Models;

namespace Commerce.Domain.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(IMongoContext context) : base(context)
        {
        }
    }
}