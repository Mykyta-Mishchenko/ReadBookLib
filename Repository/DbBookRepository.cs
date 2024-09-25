using Microsoft.EntityFrameworkCore;
using ReadBookLib.Data;
using ReadBookLib.Interfaces;
using ReadBookLib.Models;
using System.Net;

namespace ReadBookLib.Repository
{
	public class DbBookRepository : IBookRepository
	{
		private DataDbContext _dbContext;
		public DbBookRepository(DataDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		IQueryable<Book> IBookRepository.Books => _dbContext.Books;

		public async Task CreateBook(Book book)
		{
			_dbContext.Books.Add(book);
			await _dbContext.SaveChangesAsync();
		}

		public async Task DeleteBook(Book book)
		{
			_dbContext.Remove(book);
			await _dbContext.SaveChangesAsync();
		}

		public Book GetBook(int bookId)
		{
			return _dbContext.Books.FirstOrDefault(b=>b.Id == bookId);
		}

		public Book GetBook(string bookName)
		{
			return _dbContext.Books.FirstOrDefault(b => b.Name == bookName);
		}

		public async Task SaveBook(Book book)
		{
			await _dbContext.SaveChangesAsync();
		}

		public async Task UpdateBook(Book book)
		{
			_dbContext.Update(book);
			await _dbContext.SaveChangesAsync();
		}

		public int BooksCount()
		{
			return _dbContext.Books.Count();
		}

		public List<Book> GetBooksByStatus(BookStatus? status)
		{
			return _dbContext.Books.Where(b=> status == null ? true : b.Status == status).ToList();
		}
	}
}
