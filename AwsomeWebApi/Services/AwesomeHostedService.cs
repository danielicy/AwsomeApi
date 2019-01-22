
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AwsomeWebApi.Services
{
    public class AwesomeHostedService : IHostedService
    {
        private readonly IHostingEnvironment env;
        public AwesomeHostedService(IHostingEnvironment env)
        {
            this.env = env;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var client = new HttpClient();

            var file = $@"{env.ContentRootPath}\wwwroot\comments.json";
            while (true)
            {
                var response = await client.GetAsync("https://jsonplaceholder.typicode.com/photos");

                using (var output = File.OpenWrite(file))
                using (var content = await response.Content.
                ReadAsStreamAsync())
                {
                    content.CopyTo(output);
                }
                Thread.Sleep(60000);
            }
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }
    }
}
