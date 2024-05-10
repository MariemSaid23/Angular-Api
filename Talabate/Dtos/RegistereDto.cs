using System.ComponentModel.DataAnnotations;

namespace Talabate.Dtos
{
    public class RegistereDto
    {
        [Required]
        public string DisplayName { get; set; } = null!;

        [Required]
        [EmailAddress]

        public string Email { get; set; } =null!;
        [Required]

        public string Phone { get; set; } = null!;
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d\s]).{6,10}$", ErrorMessage = "Password must have at least 1 uppercase letter, 1 lowercase letter, 1 number, 1 non-alphanumeric character, and be at least 6 characters long.")]
        public string Password { get; set; } =null!;
        [Required, Compare("Password")]
        public string rePassword { get; set; } = null!;
    }
}
