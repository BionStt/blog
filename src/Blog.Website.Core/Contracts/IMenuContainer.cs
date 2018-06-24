using System.Collections.Generic;
using Blog.Website.Core.ViewModels.Author.ViewComponents;

namespace Blog.Website.Core.Contracts
{
    public interface IMenuContainer
    {
        List<MenuItemData> Items { get; set; }
    }
}