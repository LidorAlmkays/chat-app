using Common.Validators;
using System.ComponentModel.DataAnnotations;

namespace Common.DTOs
{
    public class RequestDeleteUserByEmailDTO
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StrictEmail(ErrorMessage = "Invalid email format.")]
        public required string Email { get; set; }
    }
    public class ResponseDeleteUserByEmailDTO
    {
    }
}