using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class RegisterModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
