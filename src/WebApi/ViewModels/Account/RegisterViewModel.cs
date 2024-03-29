﻿using System.ComponentModel.DataAnnotations;

namespace WebApi.ViewModels.Acconut
{
    public class RegisterViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Display(Name = "Mobile")]
        public string? Mobile { get; set; }

        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }
        public string? Role { get; set; }


    }
}
