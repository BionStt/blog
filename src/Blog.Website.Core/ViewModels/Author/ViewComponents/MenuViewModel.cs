using System;
using System.Collections.Generic;

namespace Blog.Website.Core.ViewModels.Author.ViewComponents
{
    public class MenuViewModel
    {
        public String Request { get; set; }
        public List<MenuItemData> Links { get; set; }

        public MenuViewModel(List<MenuItemData> menuItems, String request)
        {
            Request = request;
            Links = menuItems;
        }
    }
}