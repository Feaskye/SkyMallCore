using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyNetCore.Common.Middlewares
{
    public static class TokenProviderExtensions
    {
        public static IApplicationBuilder UseTokenProvider(this IApplicationBuilder builder,TokenProviderOptions options)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            return builder.UseMiddleware<TokenProviderMiddleware>(Options.Create(options));
        }

    }
}
