﻿using System.ComponentModel.DataAnnotations;

namespace Vou.Services.AuthAPI.Models.Dto
{
    public class CreateUserDto
    {

        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        public string RoleName { get; set; } = string.Empty;
    }
}
