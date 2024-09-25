using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ReadBookLib.Data;
using ReadBookLib.Interfaces;
using ReadBookLib.Models;
using ReadBookLib.Models.ViewModels;
using System.Reflection.Metadata.Ecma335;

namespace ReadBookLib.Controllers
{
	[Authorize]
	public class HomeController: Controller
	{
		private IBookRepository _bookRepository;
		private int ItemsPerPage = 20;
		public HomeController(IBookRepository bookRepository)
		{
			_bookRepository = bookRepository;
		}
		public IActionResult Index(int booksPage = 1)
		{
			return View(new BookListViewModel
			{
				Books = _bookRepository.Books
					.Where(b=> b.Status == BookStatus.Admitted)
					.OrderByDescending(b=>b.Id)
					.Skip(ItemsPerPage * (booksPage - 1))
					.Take(ItemsPerPage),
				PagingInfo = new PagingInfo
				{
					TotalItems = _bookRepository.Books.Where(b => b.Status == BookStatus.Admitted).Count(),
					ItemsPerPage = ItemsPerPage,
					CurrentPage = booksPage,
					CurrentAction = "Index"
				}
			});
		}

		[HttpGet]
		public IActionResult GetBooksByName(string name, int booksPage = 1)
		{
			if (name.IsNullOrEmpty())
			{
				return RedirectToAction("Index");
			}

			var books = _bookRepository.Books
					.Where(b => b.Status == BookStatus.Admitted)
					.Where(b => b.Name.Contains(name))
					.OrderByDescending(b => b.Id)
					.Skip(ItemsPerPage * (booksPage - 1))
					.Take(ItemsPerPage);
			if(books.Count() == 0)
			{
				books = _bookRepository.Books;
			}

			return View("Index",new BookListViewModel
			{
				Books = books,
				PagingInfo = new PagingInfo
				{
					TotalItems = _bookRepository.Books.Where(b => b.Status == BookStatus.Admitted && b.Name.Contains(name)).Count(),
					ItemsPerPage = ItemsPerPage,
					CurrentPage = booksPage,
					CurrentAction = "GetBooksByName"
				}
			});
		}

		[HttpGet]
		public IActionResult BookPage(int Id)
		{
			var book = _bookRepository.GetBook(Id);
			if(book == null) return RedirectToAction("Index");
			return View(book);
		}
	}
}
