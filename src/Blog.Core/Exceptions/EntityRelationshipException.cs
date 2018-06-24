using System;

namespace Blog.Core.Exceptions
{
    public class EntityRelationshipException : Exception
    {
        public EntityRelationshipException() { }
        
        public EntityRelationshipException(String message) : base(message) { }
    }
}