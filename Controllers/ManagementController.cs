using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ReadBookLib.Data;
using ReadBookLib.Interfaces;
using ReadBookLib.Models;
using ReadBookLib.Models.ViewModels;
using Host = Microsoft.AspNetCore.Hosting;

namespace ReadBookLib.Controllers
{
	[Authorize(Roles = "Admin")]
	public class ManagementController: Controller
	{
        private Host.IHostingEnvironment Environment { get; set; }
		private IBookRepository _bookRepository;
		private int ItemsPerPage = 20;
		public ManagementController(IBookRepository bookRepository, Host.IHostingEnvironment environment) 
		{
			_bookRepository = bookRepository;
			Environment = environment;
		}

		public ActionResult Index(BookStatus? status, int booksPage = 1)
		{
			return View(new BookListViewModel
			{
				Books = _bookRepository.Books
					.Where(b=>status == null ? true : b.Status == status)
					.OrderBy(b=>b.Status)
					.OrderByDescending(b => b.Id)
					.Skip(ItemsPerPage*(booksPage-1))
					.Take(ItemsPerPage),
				PagingInfo = new PagingInfo
				{
					TotalItems = status == null  
						? _bookRepository.BooksCount()
						: _bookRepository.GetBooksByStatus(status).Count(),
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
			return View("Index", new BookListViewModel
			{
				Books = _bookRepository.Books
					.Where(b => b.Name.Contains(name))
					.OrderBy(b => b.Status)
					.OrderByDescending(b => b.Id)
					.Skip(ItemsPerPage * (booksPage - 1))
					.Take(ItemsPerPage),
				PagingInfo = new PagingInfo
				{
					TotalItems = _bookRepository.Books.Where(b=> b.Name.Contains(name)).Count(),
					ItemsPerPage = ItemsPerPage,
					CurrentPage = booksPage,
					CurrentAction = "GetBooksByName"
				}
			});
		}
		[HttpGet]
		public ActionResult Revision(int bookId, int currentPage)
		{
			var book = _bookRepository.GetBook(bookId);
			book.StorageLocation = book.StorageLocation.Replace(Environment.ContentRootPath,"");
            return View(book);
		}
	}
}
