using System.ComponentModel.DataAnnotations;

namespace simpleAPI.Dtos
{
    public class UpdateUserDto
    {
        [Required]
        [StringLength(80, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
