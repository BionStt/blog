using System.Threading.Tasks;
using Blog.Website.Core.Contracts;
using Blog.Website.Core.ViewModels.Author.ViewComponents;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Website.ViewComponents.HeaderMenu
{
    public class HeaderMenuViewComponent : ViewComponent
    {
        private readonly IStoryEditMenuContainer _container;

        public HeaderMenuViewComponent(IStoryEditMenuContainer container)
        {
            _container = container;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(new MenuViewModel(_container.Items, Request.Path.Value));
        }
    }
}