using System;
using Blog.Core.Exceptions;
using Blog.Website.Core.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Blog.Website.Filters
{
    public class GlobalException : IExceptionFilter
    {
        private readonly ILogger _logger;

        public GlobalException(ILoggerFactory factory)
        {
            _logger = factory.GetUnhandledLogger();
        }

        public void OnException(ExceptionContext context)
        {
            _logger.UnhandledError(context.Exception);
            context.ExceptionHandled = true;

            if(context.Exception is EntityNotFoundException)
            {
                context.Result = new ViewResult {ViewName = "Error-404"};
                return;
            }
            
            context.Result = new ViewResult {ViewName = "Error-500"};
        }
    }
}