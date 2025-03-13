using System.ComponentModel.DataAnnotations;
namespace DTOs
{
    public class RequestCreateUserDTO
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters.")]
        public required string Username { get; set; }

        [Range(18, 100, ErrorMessage = "Age must be between 18 and 100.")]
        public required int Age { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public required string Password { get; set; }

        public override string ToString()
        {
            return $"Username: {Username}, Email: {Email}, Password: {Password}, Age: {Age}";
        }
    }

    public class ResponseCreateUserDTO
    {
        public required Guid Id { get; set; }
    }
}