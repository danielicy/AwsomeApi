using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AwsomeWebApi.AwsomeServer
{
    internal class AwesomeFolderWatcher<TContext>
    {
        private readonly FileSystemWatcher watcher;
        private IHttpApplication<TContext> application;
        private IFeatureCollection features;

        public AwesomeFolderWatcher(IHttpApplication<TContext> application, IFeatureCollection features)
        {
            var path = features.Get<IServerAddressesFeature>().Addresses.FirstOrDefault();
            this.watcher = new FileSystemWatcher(path);
            this.watcher.EnableRaisingEvents = true;
            this.application = application;
            this.features = features;
        }

        public void Watch()
        {
            watcher.Created += async (sender, e) =>
            {
                var context = (HostingApplication.Context)(object)application.
                CreateContext(features);
                context.HttpContext = new AwesomeHttpContext(features,e.FullPath);
                await application.ProcessRequestAsync((TContext)(object)context);
                context.HttpContext.Response.OnCompleted(null, null);
            };
            Task.Run(() => watcher.WaitForChanged(WatcherChangeTypes.All));
        }
    }

 

    public class FileHttpRequest : HttpRequest
    {
        public FileHttpRequest(HttpContext httpContext, string path)
        {
            var lines = File.ReadAllText(path).Split('\n');
            var request = lines[0].Split(' ');
            this.Method = request[0];
            this.Path = request[1];
            this.HttpContext = httpContext;
        }
        public override HttpContext HttpContext { get; }
        public override string Method { get; set; }
        public override string Scheme { get; set; } = "file";
        public override bool IsHttps { get; set; }
        public override HostString Host { get; set; }
        public override PathString PathBase { get; set; }
        public override PathString Path { get; set; }
        public override QueryString QueryString { get; set; }
        public override IQueryCollection Query { get; set; }
        public override string Protocol { get; set; }
        public override Stream Body { get; set; }
        public override IHeaderDictionary Headers => new HeaderDictionary();
        public override IRequestCookieCollection Cookies { get; set; }
        public override long? ContentLength { get; set; }
        public override string ContentType { get; set; }
        public override IFormCollection Form { get; set; }
        public override bool HasFormContentType => throw new NotImplementedException();
        public override Task<IFormCollection> ReadFormAsync(CancellationToken cancellationToken = default(CancellationToken))
            => throw new NotImplementedException();
    }
}