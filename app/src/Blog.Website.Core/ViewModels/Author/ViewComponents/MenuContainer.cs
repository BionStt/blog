using System.Collections.Generic;
using Blog.Website.Core.Contracts;
using Microsoft.Extensions.Configuration;

namespace Blog.Website.Core.ViewModels.Author.ViewComponents
{
    public abstract class MenuContainer : IMenuContainer
    {
        public List<MenuItemData> Items { get; set; }

        protected MenuContainer(IConfigurationSection section)
        {
            var items = new List<MenuItemData>();
            section.Bind(items);
            Items = items;
        }
    }
}