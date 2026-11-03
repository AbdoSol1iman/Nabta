using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Nabta.Application.Dtos.Auth
{
    public class RegisterDto
    {
       [ Required]
    [MaxLength(150)]
        public string FullName { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        public string? Email { get; set; } = null;

        [Required]
        [MinLength(8)]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } 
    }
}
