using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AwsomeWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {

            BuildCustomWebHost(args).Run();
           // BuildWebHost(args).Run();
            // CreateWebHostBuilder(args).Build().Run();
        }


        //=========================
 
        public static IWebHost BuildCustomWebHost(string[] args) =>
            new WebHostBuilder().UseKestrel()
            .UseContentRoot(Directory
                .GetCurrentDirectory())
            .ConfigureAppConfiguration(config => config.AddJsonFile("appSettings.json", true)
            )
            .ConfigureLogging(logging =>
            logging.AddConsole().AddDebug()
            )
            .UseIISIntegration()
            .UseStartup<Startup>()
            .Build(); 
        //=========================================

        /*   public static IWebHost BuildWebHost(string[] args) =>
               WebHost.CreateDefaultBuilder(args)
               .UseStartup<Startup>()
               .Build();*/

       /* public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();*/
    }
}
