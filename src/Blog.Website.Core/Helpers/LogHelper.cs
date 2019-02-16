using System;
using Microsoft.Extensions.Logging;

namespace Blog.Website.Core.Helpers
{
    public static class LogHelper
    {
        private const String UnhandledException = "Unhandled";
        private const String Default = "BlogLogger";

        public static ILogger GetUnhandledLogger(this ILoggerFactory factory)
        {
            return factory.CreateLogger(UnhandledException);
        }

        public static void UnhandledError(this ILogger logger, Exception exception)
        {
            logger.LogError(1, exception, exception.Message);
        }
        
        public static ILogger GetLogger(this ILoggerFactory factory)
        {
            return factory.CreateLogger(Default);
        }

        public static void Error(this ILogger logger, Exception exception)
        {
            logger.LogError(new EventId(1, Default), exception, exception.Message);
        }

        public static void Info(this ILogger logger, String message)
        {
            logger.LogInformation(Default, message);
        }
    }
}