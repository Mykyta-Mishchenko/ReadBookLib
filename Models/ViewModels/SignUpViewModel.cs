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
		[DataType(DataType.Password, ErrorMessage = "Incorrect or missing password")]
		public string Password { get; set; }

        public string Role { get; set; }
    }
}
