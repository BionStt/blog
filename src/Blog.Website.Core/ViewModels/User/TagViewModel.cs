using System;
using Blog.Core.Entities;

namespace Blog.Website.Core.ViewModels.User
{
    public class TagViewModel
    {
        public String Name { get; set; }
        public String Alias { get; set; }

        public TagViewModel()
        {
        }

        public TagViewModel(Tag tag)
        {
            if(tag == null)
            {
                return;
            }

            Name = tag.Name;
            Alias = tag.Alias;
        }
    }
}