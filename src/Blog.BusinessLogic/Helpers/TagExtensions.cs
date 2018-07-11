using System;
using Blog.Core.Entities;
using Blog.Core.Enums;

namespace Blog.BusinessLogic.Helpers
{
    public static class TagExtensions
    {
        public static SitemapItem ToSitemapItem(this Tag tag, String baseUrl)
        {
            return new SitemapItem
                   {
                       Url = $"{baseUrl}/{tag.Alias}",
                       ChangeFrequency = ChangeFrequency.Weekly,
                   };
        }
    }
}