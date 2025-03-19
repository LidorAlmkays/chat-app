using Common.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Common.DTOs
{
    public class RequestUpdateUserByEmailDTO
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StrictEmail(ErrorMessage = "Invalid email format.")]
        public required string Email { get; set; }
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StrictEmail(ErrorMessage = "Invalid email format.")]
        public string? NewEmail { get; set; }

        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters.")]
        public string? Username { get; set; }

        [AgeRange(18, 100, ErrorMessage = "Birthday must be between 18 and 100.")]
        public DateTime? Birthday { get; set; }

        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string? Password { get; set; }

        [ValidRole]
        public string? Role { get; set; }
    }

    public class ResponseUpdateUserByEmailDTO
    {

    }
}