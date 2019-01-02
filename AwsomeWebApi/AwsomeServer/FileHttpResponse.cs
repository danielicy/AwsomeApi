using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AwsomeWebApi.AwsomeServer
{
    internal class FileHttpResponse : HttpResponse
    {
        private AwesomeHttpContext awesomeHttpContext;
        private string path;

        public FileHttpResponse(AwesomeHttpContext awesomeHttpContext, string path)
        {
            this.awesomeHttpContext = awesomeHttpContext;
            this.path = path;
        }

        public override HttpContext HttpContext { get; }
        public override int StatusCode { get; set; }
        public override IHeaderDictionary Headers => new HeaderDictionary();
        public override Stream Body { get; set; } = new MemoryStream();
        public override long? ContentLength { get; set; }
        public override string ContentType { get; set; }
        public override IResponseCookies Cookies => throw new NotImplementedException();
        public override bool HasStarted => true;

        public override void OnCompleted(Func<object, Task> callback, object state)
        {
            using (var reader = new StreamReader(this.Body))
            {
                this.Body.Position = 0;
                var text = reader.ReadToEnd();
                 File.WriteAllText(path, $"{this.StatusCode} - {text}");
                this.Body.Flush();
                this.Body.Dispose();
            }
        }



        public override void OnStarting(Func<object, Task> callback, object state) { }
        public override void Redirect(string location, bool permanent) { }
    }
}