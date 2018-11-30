using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.File;
using System;

namespace Microsoft.Extensions.Logging
{
    /// <summary>
    /// ILogger¿©’πFileLogger
    /// https://github.com/huanent/Huanent.Logging
    /// </summary>
    public static class FileLoggerFactoryExtensions
    {

        public static ILoggingBuilder AddFile(this ILoggingBuilder builder)
        {
            builder.Services.AddSingleton<ILoggerProvider, FileLoggerProvider>();
            return builder;
        }


        public static ILoggerFactory AddFile(this ILoggerFactory factory)
        {
            return AddFile(factory, LogLevel.Information);
        }


        public static ILoggerFactory AddFile(this ILoggerFactory factory, Func<string, LogLevel, bool> filter)
        {
            factory.AddProvider(new FileLoggerProvider(filter));
            return factory;
        }

        public static ILoggerFactory AddFile(this ILoggerFactory factory, LogLevel minLevel)
        {
            return AddFile(
               factory,
               (_, logLevel) => logLevel >= minLevel);
        }
    }
}