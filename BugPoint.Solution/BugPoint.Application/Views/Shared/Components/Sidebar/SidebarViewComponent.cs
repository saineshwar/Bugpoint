using Microsoft.AspNetCore.Mvc;

namespace BugPoint.Application.Views.Shared.Components.Sidebar
{
    public class SidebarViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke() => View();
    }
}