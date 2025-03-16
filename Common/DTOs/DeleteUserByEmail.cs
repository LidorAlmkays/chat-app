using System.ComponentModel.DataAnnotations;

namespace Common.DTOs
{
    public class RequestDeleteUserByEmailDTO
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
    }
    public class ResponseDeleteUserByEmailDTO
    {
    }
}