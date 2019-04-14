using Blog.Website.Core.ConfigurationOptions;
using Blog.Website.Core.Contracts;
using Microsoft.Extensions.Options;

namespace Blog.Website.Core.ViewModels.Author.ViewComponents
{
    public class MainMenuContainer : MenuContainer,
                                     IMainMenuContainer
    {
        public MainMenuContainer(IOptions<MainMenuOptions> options) : base(options.Value?.Main)
        {
        }
    }
}