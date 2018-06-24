using System;

namespace Blog.Website.Core.Models
{
    public class LoginRestriction
    {
        public String ProviderName { get; set; }
        public String Login { get; set; }
        public String PrincipalType { get; set; }
    }
}