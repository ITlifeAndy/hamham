using System.ComponentModel.DataAnnotations;

namespace HamHam.Api.Models
{
    public class UserProfileUpdateDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string? Password { get; set; }

        public string? AvatarUrl { get; set; }
    }
}
