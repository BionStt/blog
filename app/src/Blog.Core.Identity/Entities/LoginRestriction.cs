using System;

namespace Blog.Core.Identity.Entities
{
    public class LoginRestriction
    {
        public String ProviderName { get; set; }
        public String Login { get; set; }
        public String PrincipalType { get; set; }
    }
}