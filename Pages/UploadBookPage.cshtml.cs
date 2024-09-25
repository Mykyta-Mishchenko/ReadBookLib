using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReadBookLib.Interfaces;
using ReadBookLib.Models;
using System.ComponentModel.DataAnnotations;
using System.Reflection.PortableExecutable;

namespace ReadBookLib.Pages
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class UploadBookPageModel : PageModel
    {
        [Required]
        [BindProperty]
        public string Name { get; set; }
		[Required]
		[BindProperty]
		public string Description { get; set; }
		[Required]
		[BindProperty]
		public IFormFile File {  get; set; }


		private readonly IFileService _fileService;
        private readonly IBookRepository _bookRepository;
		public UploadBookPageModel(IFileService fileService, IBookRepository bookRepository)
        {
            _fileService = fileService;
            _bookRepository = bookRepository;
        }
        [RequestFormLimits(MultipartBodyLengthLimit = 10485760)]
        public async Task<IActionResult> OnPostAsync() 
        {
            if(File != null)
            {
				var postedFileExtension = Path.GetExtension(File.FileName);
				if (postedFileExtension != ".pdf")
				{
					ModelState.AddModelError("File", "Incorrect file extension! Please upload only pdf!");
				}
				if (ModelState.IsValid)
                {
					var filePath = await _fileService.UploadFile(File);

					PdfReader pdfReader = new PdfReader(filePath);
					int numberOfPages = pdfReader.NumberOfPages;

                    var Book = new Book
                    {
                        Name = Name,
                        Description = Description,
                        StorageLocation = filePath,
                        Status = BookStatus.Uploaded,
                        StatusTime = DateTime.Now,
                        TotalPagesNum = numberOfPages,
                        Language = ""
                    };
                    if(ModelState.IsValid)
                    {
						await _bookRepository.CreateBook(Book);
						return Redirect("default");
					}
                    else
                    {
                        ModelState.AddModelError("File", "Try again!");
                    }
				}
                else
                {
                    ModelState.AddModelError("File", "Something went wrong!");
                }
			}
            else
            {
				ModelState.AddModelError("File", "Select File!");
			}
            return Page();
        }
    }
}
