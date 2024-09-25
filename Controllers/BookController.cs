using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReadBookLib.Data;
using ReadBookLib.Interfaces;
using ReadBookLib.Models;
using ReadBookLib.Models.ViewModels;
using ReadBookLib.Repository;
using System.Reflection.Metadata.Ecma335;

namespace ReadBookLib.Controllers
{
    [Authorize]
    public class BookController : Controller
    {
        private IBookRepository _bookRepository;
        public BookController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpGet]
        public IActionResult Index([FromQuery]int bookId)
        {
            var book = _bookRepository.GetBook(bookId);
            if (book == null) return RedirectToAction("Index","Home");

			return View(book);
        }
        [HttpGet]
        public IActionResult GetPdf(int Id)
        {
            var path = _bookRepository.GetBook(Id).StorageLocation ?? null;
            if(path == null)
            {
                return NotFound();
            }
			var pdfBytes = System.IO.File.ReadAllBytes(path);
			return File(pdfBytes, "application/pdf");
		}
        [HttpPost]
        public async Task<IActionResult> Update(BookViewModel book)
        {
            Book dbBook = _bookRepository.GetBook(book.Id);
            dbBook.Name = book.Name;
            dbBook.Description = book.Description;
            if(dbBook.Status != book.Status)
            {
                dbBook.Status = book.Status;
                dbBook.StatusTime = DateTime.Now;
            }
            await _bookRepository.UpdateBook(dbBook);
            return RedirectToAction("Index","Management");
        }
    }
}
