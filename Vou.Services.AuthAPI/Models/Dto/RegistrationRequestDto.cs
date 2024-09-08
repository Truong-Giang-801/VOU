﻿using System.ComponentModel.DataAnnotations;

namespace Vou.Services.AuthAPI.Models.Dto
{
    public class RegistrationRequestDto
    {

        public string Name { get; set; }

        // At least one of Email or PhoneNumber is required, but both are not mandatory.
        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        [Required]
        public string Password { get; set; }

        public string? RoleName { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(Email) && string.IsNullOrEmpty(PhoneNumber))
            {
                yield return new ValidationResult("Either Email or PhoneNumber must be provided.", new[] { nameof(Email), nameof(PhoneNumber) });
            }
        }
    }
}
