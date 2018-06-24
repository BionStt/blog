using System;

namespace Blog.Core.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException() { }

        public EntityNotFoundException(String message) : base(message) { }
    }
}