using System.ComponentModel.DataAnnotations;

namespace customer.write.Inputs
{
    public record CustomerInput(
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        string Name,
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        string Surname
    );
}