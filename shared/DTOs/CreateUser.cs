using System.ComponentModel.DataAnnotations;
namespace DTOs
{
    public class RequestCreateUserDTO
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public required string UserName { get; set; }

        [Range(18, 100, ErrorMessage = "Age must be between 18 and 100.")]
        public required int Age { get; set; }
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        [Required]
        [MinLength(8)]
        public required string Password { get; set; }
    }
    public class ResponseCreateUserDTO
    {
        public required int Id { get; set; }
    }
}