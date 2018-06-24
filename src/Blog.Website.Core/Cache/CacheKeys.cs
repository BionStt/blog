using System;

namespace Blog.Website.Core.Cache
{
    public static class CacheKeys
    {
        public static String StoryPrefix => "story-";
        public static String StoriesPagePrefix => "storiesPage-";
        public static String CategoryStoriesPagePrefix => "category-storiesPage-";
        public static String TagStoriesPagePrefix => "tag-storiesPage-";
        public static String DefaultPagesStories => "dafeault-pages-stories";
        public static String CategoryPagesStories => "category-pages-stories";
        public static String TagPagesStories => "tag-pages-stories";

        public static String Tags => "tags";
        public static String Categories => "categories";
    }
}