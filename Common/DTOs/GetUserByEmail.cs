using System.ComponentModel.DataAnnotations;

namespace Common.DTOs
{
    public class RequestGetUserByEmailDTO
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
    }
    public class ResponseGetUserByEmailDTO
    {
        public Guid? Id { get; set; }

        public string Email { get; set; }

        public DateTime? Birthday { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }

        public string PasswordKey { get; set; }

        public DateTimeOffset? CreatedAt { get; set; }
    }
}