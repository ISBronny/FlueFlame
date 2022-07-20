using System;
using System.Net.Http;
using FlueFlame.AspNetCore.Common;
using FlueFlame.AspNetCore.Modules.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;

namespace FlueFlame.AspNetCore.Modules.Http
{
    public class HttpModule : AspNetModuleBase
    {
        private Action<HttpContext> ConfigureHttpContext
        {
            set => Application.HttpService.ConfigureHttpContext(value);
        }

        public HttpModule(FlueFlameHost application, HttpMethod httpMethod) : base(application)
        {
            Application.HttpService.SetMethod(httpMethod);
        }

        public HttpModule Send()
        {
            Application.Send();
            return this;
        }
        
        public HttpModule Url(string url)
        {
            ConfigureHttpContext = x => x.Request.Path = url;
            return this;
        }
        
        public HttpModule WithHeader(string key, string value)
        {
            ConfigureHttpContext = x => x.Request.Headers.Add(key, value);
            return this;
        }

        public HttpModule Accepts(string accepts)
        {
            ConfigureHttpContext = x => x.Request.Headers.Accept = accepts;
            return this;
        }

        public HttpModule QueryParam(string paramName, string paramValue)
        {
            ConfigureHttpContext = context =>
            {
                context.Request.QueryString = context.Request.QueryString.Add(paramName, paramValue);
                context.Request.Query =
                    new QueryCollection(QueryHelpers.ParseQuery(context.Request.QueryString.Value!));
            };
            return this;
        }

        public HttpModule Json(object body)
        {
            Application.HttpService.AddJson(body);
            return this;
        }
        
        public HttpModule Xml(object body)
        {
            Application.HttpService.AddXml(body);
            return this;
        }

        public HttpModule Text(string text)
        {
            Application.HttpService.AddText(text);
            return this;
        }
        
        public ResponseModule Response => new(Application);
    }
}