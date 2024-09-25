using System.ComponentModel.DataAnnotations;

namespace ReadBookLib.Models.ViewModels
{
    public class SignInViewModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
		[MinLength(8, ErrorMessage = "Enter at least 8 charachters")]
		[RegularExpression(@"^[a-zA-Z0-9_$&#?!-]*$", ErrorMessage = "Use letters, numbers, and special symbols like _$&#?-")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
