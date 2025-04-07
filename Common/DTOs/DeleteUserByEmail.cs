using Common.Validators;
using System.ComponentModel.DataAnnotations;

namespace Common.DTOs
{
    public record RequestDeleteUserByEmailDTO
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StrictEmail(ErrorMessage = "Invalid email format.")]
        public required string Email { get; set; }
    }
    public record ResponseDeleteUserByEmailDTO
    {
    }
}