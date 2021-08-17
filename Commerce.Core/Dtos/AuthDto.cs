namespace Commerce.Core.Dtos
{
    public class RegisterDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
    }
    
    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}