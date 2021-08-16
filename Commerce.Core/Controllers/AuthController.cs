using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Commerce.Domain.Interfaces;
using Commerce.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Commerce.Core.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IUnitOfWork _uow;
        private readonly IUserRepository _userRepository;

        public AuthController(IUserRepository userRepository, IUnitOfWork uow, ILogger<AuthController> logger)
        {
            _logger = logger;
            _userRepository = userRepository;
            _uow = uow;
        }
        
        [HttpPost]
        public async Task<Boolean> CreateNewUser()
        {
            User user = new User("Heo con", "heocon@gmail.com", "1234567890");
            
            _userRepository.Add(user);
            
            await _uow.Commit();
            
            return true;
        }
        
        
        [HttpGet]
        public Task<IEnumerable<User>> GetAllUsers()
        {
            return _userRepository.GetAll();
        }
    }
}