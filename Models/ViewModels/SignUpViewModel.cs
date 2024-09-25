using System.ComponentModel.DataAnnotations;

namespace ReadBookLib.Models.ViewModels
{
    public class SignUpViewModel
    {
        [Required]
        [MinLength(4)]
        public string FirstName { get; set; }
        [Required]
        [MinLength(4)]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
		[DataType(DataType.Password)]
		[MinLength(8, ErrorMessage="Enter at least 8 charachters")]
		[RegularExpression(@"^[a-zA-Z0-9_$&#?!-]*$", ErrorMessage = "Use letters, numbers, and special symbols like _$&#?-")]
		public string Password { get; set; }

        public string Role { get; set; }
    }
}
