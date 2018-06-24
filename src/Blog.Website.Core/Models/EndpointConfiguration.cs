using System;

namespace Blog.Website.Core.Models
{
    public class EndpointConfiguration
    {
        public String Host { get; set; }
        public Int32? Port { get; set; }
        public String Scheme { get; set; }
        public String StoreName { get; set; }
        public String StoreLocation { get; set; }
        public String FilePath { get; set; }
        public String Password { get; set; }
    }
}