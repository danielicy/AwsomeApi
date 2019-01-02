using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace AwsomeWebApi
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddSingleton<IComponent, ComponentB>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
         public void Configure(IApplicationBuilder app, IHostingEnvironment env)
         {
             if (env.IsDevelopment())
             {
                 app.UseDeveloperExceptionPage();
             }

            app.Map("/skip", (skipApp) => skipApp.Run(async (context) => 
            await context.Response.WriteAsync($"Skip the line!")));

            app.Use(async (context, next) =>
            {
                var value = context.Request.Query["value"].ToString();
                if (int.TryParse(value, out int intValue))
                {
                    await context.Response.WriteAsync($"You entered a number:{intValue}");
                }
                else
                {
                    context.Items["value"] = value;
                    await next();
                }
            });

            app.Use(async (context, next) =>
            {
                var value = context.Items["value"].ToString();
                if (int.TryParse(value, out int intValue))
                {
                    await context.Response.WriteAsync($"You entered a number:{ intValue}");
}
                else
                {
                    await next();
                }
            });

            app.Use(async (context, next) =>
            {
                var value = context.Items["value"].ToString();
                context.Items["value"] = value.ToUpper();
                await next();
            });
            app.Use(async (context, next) =>
            {
                var value = context.Items["value"].ToString();
                context.Items["value"] = Regex.Replace(value, "(?<!^)[AEUI](?!$)",
                "*");
                await next();
            });
            
            //==================================================

            app.Map("/foo",
                config =>
                config.Use(async (context, next) =>
                await context.Response.WriteAsync("<div><h1>Mexico</h1>" +
                "<p>Macho Man</p></div>")
                )
                );
            app.MapWhen(
                context =>
                context.Request.Method == "POST" &&
                context.Request.Path == "/bar",
                config =>
                config.Use(async (context, next) =>
                await context.Response.WriteAsync("Welcome to POST /bar")
                )
                );
            /*app.Run(async (context) =>
            await context.Response.WriteAsync($"Welcome to the default")
            )*/


            /*  app.Run(async (context) =>
              {
                  await context.Response.WriteAsync("Hello World!");
              });*/
            app.Run(async (context) =>
            {
                var value = context.Items["value"].ToString();
                await context.Response.WriteAsync($"You entered a string: {value}");
            });
        }

       /* public void Configure(IApplicationBuilder app, IComponent component)
        {
            app.Run(async (context) =>
            await context.Response.WriteAsync($"Name is {component.Name}")
            );
        }*/




    }



    public interface IComponent
    {
        string Name { get; }
    }

    public class ComponentA
    {
        private readonly IComponent _componentB;
        public ComponentA(IComponent componentB)
        {
            this._componentB = componentB;
        }
    }
    public class ComponentB : IComponent
    {
        public string Name { get; set; } = nameof(ComponentB);
    }


   

}
