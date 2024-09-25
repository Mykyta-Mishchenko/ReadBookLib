using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ReadBookLib.Models;

namespace ReadBookLib.Data
{
	public class DataDbContext: IdentityDbContext<User>
	{
		public DataDbContext(DbContextOptions options) : base(options) { }

		public DbSet<Book> Books => Set<Book>();
	}
}
