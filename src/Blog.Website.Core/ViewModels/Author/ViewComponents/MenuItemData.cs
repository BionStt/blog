using System;

namespace Blog.Website.Core.ViewModels.Author.ViewComponents
{
    public class MenuItemData
    {
        public String Id { get; set; }
        public String Text { get; set; }
        public String Url { get; set; }
        public String Icon { get; set; }

        public MenuItemData() { }

        public MenuItemData(String text, String url)
        {
            Text = text;
            Url = url;
        }
    }
}