using System.ComponentModel.DataAnnotations;

namespace DTOs
{
    public class DeleteUserByEmailDTO
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
    }
}