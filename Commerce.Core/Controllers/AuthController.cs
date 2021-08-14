using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Commerce.Core.Models;
using Commerce.Core.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Commerce.Core.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<AuthController> _logger;
        private readonly IMongoUnitOfWork _mongoUnitOfWork;

        public AuthController(IMongoUnitOfWork mongoUnitOfWork, ILogger<AuthController> logger)
        {
            _logger = logger;
            _mongoUnitOfWork = mongoUnitOfWork;
        }
        
        [HttpPost]
        public async Task<Boolean> CreateNewUser()
        {
            var repo = _mongoUnitOfWork.GetRepository<User>();
            User user = new User("Abel Tran", "abeltran.develop@gmail.com", "1234567890");
            await repo.AddAsync(user);
            
            return true;
        }
        
        
        [HttpGet]
        public List<User> GetAllUsers()
        {
            var repo = _mongoUnitOfWork.GetRepository<User>();
            return repo.FindAll(1, 50); 
        }
    }
}