using System;

namespace Blog.Data.Mongo.Entities
{
    public class Tag
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public String Alias { get; set; }

        public String SeoTitle { get; set; }
        public String SeoDescription { get; set; }
        public String SeoKeywords { get; set; }

        public Boolean IsPublished { get; set; }
    }
}