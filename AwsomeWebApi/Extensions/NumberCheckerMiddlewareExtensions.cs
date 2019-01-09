using AwsomeWebApi.Components;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwsomeWebApi.Extensions
{
    public static class NumberCheckerMiddlewareExtensions
    {
        public static IApplicationBuilder UseNumberChecker(this IApplicationBuilder app)
        {
            return app.UseMiddleware<NumberCheckerMiddleware>();
        }

        public static IApplicationBuilder UseUpperValue(this IApplicationBuilder app)
        {
            return app.UseMiddleware<UpperValueMiddleware>();
        }

        public static IApplicationBuilder UseVowelMasker(this IApplicationBuilder app)
        {
            return app.UseMiddleware<VowelMaskerMiddleware>();
        }

        public static IApplicationBuilder UseSkipApp(this IApplicationBuilder app)
        {
            return app.UseMiddleware<UseSkipMiddleware>();
        }
    }


}

