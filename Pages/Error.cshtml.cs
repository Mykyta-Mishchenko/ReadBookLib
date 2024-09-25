using Azure.Core;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace ReadBookLib.Pages
{
	[IgnoreAntiforgeryToken]
	public class ErrorModel : PageModel
    {
		public void OnGet()
		{
			
		}
	}
}
