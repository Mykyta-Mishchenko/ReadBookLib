using ReadBookLib.Models;
using System.Linq;

namespace ReadBookLib.Interfaces
{
	public interface IBookRepository
	{
		IQueryable<Book> Books { get; }
		Book GetBook(int bookId);
		Book GetBook(string bookName);
		List<Book> GetBooksByStatus(BookStatus? status);
		int BooksCount();
		Task CreateBook(Book book);
		Task UpdateBook(Book book);
		Task DeleteBook(Book book);
		Task SaveBook(Book book);
	}
}
