using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace ReadBookLib.Models.ViewModels
{
    public class BookViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter book name!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter book description!")]
        [MaxLength(500)]
        public string Description { get; set; }
        [Required(ErrorMessage = "Specify book language!")]
        public BookStatus Status { get; set; }
    }
}
