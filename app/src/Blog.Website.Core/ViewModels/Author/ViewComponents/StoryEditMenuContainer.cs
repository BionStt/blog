using Blog.Website.Core.Contracts;
using Microsoft.Extensions.Configuration;

namespace Blog.Website.Core.ViewModels.Author.ViewComponents
{
    public class StoryEditMenuContainer : MenuContainer, IStoryEditMenuContainer
    {
        public StoryEditMenuContainer(IConfigurationSection section) : base(section) { }
    }
}