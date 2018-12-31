using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
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
        public override string Method { get; set; }
        public override PathString Path { get; set; }
        public override string Scheme { get; set; } = "file";
        public override HttpContext HttpContext { get; }
        public override bool IsHttps { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public override HostString Host { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public override PathString PathBase { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public override QueryString QueryString { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public override IQueryCollection Query { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public override string Protocol { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public override IHeaderDictionary Headers => throw new System.NotImplementedException();

        public override IRequestCookieCollection Cookies { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public override long? ContentLength { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public override string ContentType { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public override Stream Body { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public override bool HasFormContentType => throw new System.NotImplementedException();

        public override IFormCollection Form { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public override Task<IFormCollection> ReadFormAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }
    }
}