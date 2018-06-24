using System;
using System.ComponentModel.DataAnnotations;

namespace Blog.Website.Core.Requests
{
    public class TagCreateRequest
    {
        [Required]
        [MinLength(3)]
        public String Name { get; set; }
    }
}