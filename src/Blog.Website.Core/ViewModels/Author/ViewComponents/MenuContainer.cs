using System.Collections.Generic;
using Blog.Website.Core.Contracts;

namespace Blog.Website.Core.ViewModels.Author.ViewComponents
{
    public abstract class MenuContainer : IMenuContainer
    {
        public List<MenuItemData> Items { get; set; }

        protected MenuContainer(List<MenuItemData> items)
        {
            Items = items ?? new List<MenuItemData>(0);
        }
    }
}