using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReadBookLib.Models
{
	public class Book
	{
		[BindNever]
		public int Id { get; set; }
		[Required(ErrorMessage = "Please enter book name!")]
		public string Name { get; set; }
		[Required(ErrorMessage = "Please enter book description!")]
		[MaxLength(500)]
		public string Description { get; set; }
		[Required(ErrorMessage = "Specify book language!")]
		public string Language { get; set; }
		[Required(ErrorMessage = "Specify book pages!")]
		public int TotalPagesNum { get; set; }
		[Required(ErrorMessage = "Specify storage location!")]
		[MaxLength(200)]
		public string StorageLocation { get; set; }
		[Required(ErrorMessage = "Specify book status!")]
		public BookStatus Status { get; set; }
		[Required(ErrorMessage = "Specify datetime os status changes!")]
		public DateTime StatusTime { get; set; }
 	}
}
