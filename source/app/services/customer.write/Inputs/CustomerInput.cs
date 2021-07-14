using System.ComponentModel.DataAnnotations;

namespace customer.write.Inputs
{
    public class CustomerInput
    {
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string Name { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string Surname { get; set; }
    }
}