using System;
using System.ComponentModel.DataAnnotations;
using Blog.Core.Entities;

namespace Blog.Website.Core.Requests
{
    public class TagCreateRequest
    {
        [Required]
        [MinLength(3)]
        public String Name { get; set; }

        public String Alias { get; set; }

        public Tag ToDomain()
        {
            return new Tag(Name, Alias);
        }
    }
}