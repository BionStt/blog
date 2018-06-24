using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Website.Core.ViewModels.User;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Website.ViewComponents.PublicMainMenu
{
    public class PublicMainMenuViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(List<TagViewModel> tags)
        {
            if (tags == null)
            {
                tags = new List<TagViewModel>(0);
            }

            return View("Default", tags);
        }
    }
}