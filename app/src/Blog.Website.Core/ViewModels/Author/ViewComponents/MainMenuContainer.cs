using Blog.Website.Core.Contracts;
using Microsoft.Extensions.Configuration;

namespace Blog.Website.Core.ViewModels.Author.ViewComponents
{
    public class MainMenuContainer : MenuContainer, IMainMenuContainer
    {
        public MainMenuContainer(IConfigurationSection section) : base(section) { }
    }
}