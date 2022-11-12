using System.Net.Http;
using FlueFlame.AspNetCore.Common;
using FlueFlame.AspNetCore.Modules.Http;

namespace FlueFlame.AspNetCore.Facades
{
    public class HttpFacade : FlueFlameFacadeBase
    {
        public HttpFacade(IFlueFlameHost application) : base(application)
        {
            
        }


        public HttpModule Delete => new(Application, HttpMethod.Delete);
        public HttpModule Get => new(Application, HttpMethod.Get);
        public HttpModule Head => new(Application, HttpMethod.Head);
        public HttpModule Options => new(Application, HttpMethod.Options);
        public HttpModule Patch => new(Application, HttpMethod.Patch);
        public HttpModule Post => new(Application, HttpMethod.Post);
        public HttpModule Put => new(Application, HttpMethod.Put);
        public HttpModule Trace => new(Application, HttpMethod.Trace);
    }
}