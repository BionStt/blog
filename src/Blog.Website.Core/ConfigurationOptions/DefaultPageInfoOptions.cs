using System;

namespace Blog.Website.Core.ConfigurationOptions
{
    public class DefaultPageInfoOptions
    {
        public String Title { get; set; }
        public String Description { get; set; }
        public String Keywords { get; set; }
        public String TagTitle { get; set; }
        public String PageNumberTitle { get; set; }
        public Int32 PageSize { get; set; }
    }
}