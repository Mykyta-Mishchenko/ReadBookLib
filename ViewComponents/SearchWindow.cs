using Microsoft.AspNetCore.Mvc;
using ReadBookLib.Models.ViewModels;

namespace ReadBookLib.ViewComponents
{
	public class SearchWindow : ViewComponent
	{
		public IViewComponentResult Invoke(PagingInfo pagingInfo, string controllerName, string actionName)
		{
			return View(new SearchWindowViewModel
			{
				Controller = controllerName,
				Action = actionName,
				CurrentPage = pagingInfo.CurrentAction == actionName ? pagingInfo.CurrentPage : 1
			});
		}
	}
}
