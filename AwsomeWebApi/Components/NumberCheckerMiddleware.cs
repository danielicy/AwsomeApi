using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AwsomeWebApi.Components
{
    public class NumberCheckerMiddleware
    {
        private readonly RequestDelegate next;
        public NumberCheckerMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var value = context.Request.Query["value"].ToString();
            if (int.TryParse(value, out int intValue))
            {
                await context.Response.WriteAsync($"You entered a number: { intValue} ");
            }
            else
            {
                context.Items["value"] = value;
                await next(context);
            }
        }
    }

    //=======================================
    public class UpperValueMiddleware
    {
        private readonly RequestDelegate next;
        public UpperValueMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            
                var value = context.Items["value"].ToString();
                context.Items["value"] = value.ToUpper();
                await next(context);
           
             
        }
    }

    //====================================
    public class VowelMaskerMiddleware
    {
        private readonly RequestDelegate next;
        public VowelMaskerMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {

            var value = context.Items["value"].ToString();
            context.Items["value"] = Regex.Replace(value, "(?<!^)[AEOUI](?!$)", "*");
            await next(context);


        }
    }

    //=======================================
    public class UseSkipMiddleware
    {
        private readonly RequestDelegate next;
        public UseSkipMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            await context.Response.WriteAsync($"Skip the line!");
        }
    }
}
