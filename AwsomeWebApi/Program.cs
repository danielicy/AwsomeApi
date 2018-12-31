using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AwsomeWebApi.AwsomeServer;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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
            ).UseServer(awsomeServer)
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
