using System.Net.Http;
using FlueFlame.AspNetCore.Common;
using FlueFlame.AspNetCore.Modules.Http;
using FlueFlame.AspNetCore.Services;
using Microsoft.Extensions.Primitives;

namespace FlueFlame.AspNetCore.Facades
{
    public class HttpFacade : FlueFlameFacadeBase
    {
        private HttpService HttpService;
        internal HttpFacade(IFlueFlameHost application, HttpService httpService) : base(application)
        {
            HttpService = httpService;
        }


        public HttpModule Delete => new(Application, HttpMethod.Delete);
        public HttpModule Get => new(Application, HttpMethod.Get);
        public HttpModule Head => new(Application, HttpMethod.Head);
        public HttpModule Options => new(Application, HttpMethod.Options);
        public HttpModule Patch => new(Application, HttpMethod.Patch);
        public HttpModule Post => new(Application, HttpMethod.Post);
        public HttpModule Put => new(Application, HttpMethod.Put);
        public HttpModule Trace => new(Application, HttpMethod.Trace);


        public HttpFacade AddDefaultHeader(string key, string value)
        {
            HttpService.ConfigureHttpContextPermanently(context =>
            {
                context.Request.Headers[key] = value;
            });
            return this;
        }
        
        public HttpFacade AddDefaultHeader(string key, StringValues values)
        {
            HttpService.ConfigureHttpContextPermanently(context =>
            {
                context.Request.Headers[key] = values;
            });
            return this;
        }
        
        public HttpFacade AddDefaultBearerToken(string token)
        {
            token = token.Replace("Bearer ", "");
            HttpService.ConfigureHttpContextPermanently(x => x.Request.Headers["Authorization"] = $"Bearer {token}");
            return this;
        }

        public HttpFacade Reset()
        {
            HttpService.Reset();
            HttpService.ResetPermanently();
            return this;
        }
    }
}