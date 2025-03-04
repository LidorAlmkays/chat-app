namespace Domain.models
{
    public class UserModel
    {
        public int Id { get; set; }
        public required string UserName { get; set; }
        public required int Age { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Role { get; set; }
        public required string PasswordKey { get; set; }
    }
}