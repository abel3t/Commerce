using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Runtime;
using Commerce.Core.Dtos;
using Commerce.Core.Filters;
using Commerce.Domain.Interfaces;
using Commerce.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;
        private readonly string _appClientId;
        private readonly string _userPoolId;
        private readonly AmazonCognitoIdentityProviderClient _cognito;

        public AuthController(IUserRepository userRepository, IUnitOfWork uow, ILogger<AuthController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _userRepository = userRepository;
            _uow = uow;
            _configuration = configuration;
            
            string accessKey = _configuration["AWS:AccessKey"];
            string secretKey = _configuration["AWS:SecretKey"];
            _appClientId = _configuration["AWS:AppClientId"];
            _userPoolId = _configuration["AWS:UserPoolId"];
            
            _cognito = new AmazonCognitoIdentityProviderClient(new BasicAWSCredentials(accessKey, secretKey), RegionEndpoint.APSoutheast1);
        }
        
        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<string>> Register(RegisterDto userDto)
        {

            var request = new SignUpRequest
            {
                ClientId = _configuration["AWS:AppClientId"],
                Password = userDto.Password,
                Username = userDto.Email
            };
            
            var emailAttribute = new AttributeType
            {
                Name = "email",
                Value = userDto.Email
            };
            request.UserAttributes.Add(emailAttribute);
            
            var response = await _cognito.SignUpAsync(request);
            
            return Ok();
        }


        [HttpPost("api/login")]
        public async Task<ActionResult<string>> Login(LoginDto userDto)
        {
            var request = new InitiateAuthRequest
            {
                ClientId = _appClientId,
                AuthFlow = AuthFlowType.USER_PASSWORD_AUTH
            };
            
            request.AuthParameters.Add("USERNAME", userDto.Email);
            request.AuthParameters.Add("PASSWORD", userDto.Password);

            var response = await _cognito.InitiateAuthAsync(request);
            
            return Ok(response.AuthenticationResult.IdToken);
        }
        
        
        [ApiKeyAuthAttribute]
        [HttpGet("api/users")]
        public List<User> GetAllUsers()
        {
            return _userRepository.FindAll(1, 50);
        }
    }
}