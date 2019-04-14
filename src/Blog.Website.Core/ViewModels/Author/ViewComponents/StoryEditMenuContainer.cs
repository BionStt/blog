using Blog.Website.Core.ConfigurationOptions;
using Blog.Website.Core.Contracts;
using Microsoft.Extensions.Options;

namespace Blog.Website.Core.ViewModels.Author.ViewComponents
{
    public class StoryEditMenuContainer : MenuContainer,
                                          IStoryEditMenuContainer
    {
        public StoryEditMenuContainer(IOptions<StoryEditMenuOptions> options) : base(options.Value?.StoryEdit)
        {
        }
    }
}