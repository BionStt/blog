using System;
using Blog.Website.Core.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Blog.Website.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger _logger;

        public GlobalExceptionFilter(ILoggerFactory factory)
        {
            _logger = factory.GetUnhandledLogger();
        }

        public void OnException(ExceptionContext context)
        {
            _logger.UnhandledError(context.Exception);
            context.ExceptionHandled = true;
            context.Result = new ViewResult {ViewName = "Error-500"};
        }
    }
}