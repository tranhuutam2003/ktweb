using Microsoft.AspNetCore.Mvc;
using Tam_222631136.Models;

namespace Tam_222631136.Viewcomponents
{
	public class NavItemsViewComponent : ViewComponent
	{
		private readonly NewShopContext db;
		List<NavItem> navItems;

		public NavItemsViewComponent(NewShopContext _context)
		{
			db = _context;
			navItems = db.NavItems.ToList();
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			return View("RenderNavItems",navItems);
		}
	}
}
