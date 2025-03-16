
namespace Gateway.Domain.models
{
    public class UserModel
    {

        public Guid? Id { get; set; }

        public string Email { get; set; }

        public DateTime? Birthday { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }

        public string PasswordKey { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}