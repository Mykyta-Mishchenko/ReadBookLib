using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ReadBookLib.Models
{
	public class User: IdentityUser
	{
		[Required(ErrorMessage = "Please enter user first name!")]
		public string FirstName { get; set; }
		[Required(ErrorMessage = "Please enter user last name!")]
		public string LastName { get; set; }
	}
}
