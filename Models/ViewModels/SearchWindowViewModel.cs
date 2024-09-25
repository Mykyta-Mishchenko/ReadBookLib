using System.Globalization;

namespace ReadBookLib.Models.ViewModels
{
	public class SearchWindowViewModel
	{
		public string Controller {  get; set; }
		public string Action { get; set; }
		public int CurrentPage { get; set; }
	}
}
