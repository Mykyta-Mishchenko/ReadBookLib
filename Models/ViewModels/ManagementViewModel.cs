namespace ReadBookLib.Models.ViewModels
{
	public class ManagementViewModel
	{
		public IEnumerable<Book> Books { get; set; } = Enumerable.Empty<Book>();
		public PagingInfo PagingInfo { get; set; }
		public BookStatus? CurrentStatus { get; set; }
	}
}
