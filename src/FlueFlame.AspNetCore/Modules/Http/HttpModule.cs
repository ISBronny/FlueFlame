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

        internal HttpModule(FlueFlameHost application, HttpMethod httpMethod) : base(application)
        {
            Application.HttpService.SetMethod(httpMethod);
        }

        /// <summary>
        /// Sends an HTTP request.
        /// </summary>
        /// <returns></returns>
        public HttpModule Send()
        {
            Application.Send();
            return this;
        }
        
        /// <summary>
        /// Sets the HTTP request path.
        /// </summary>
        /// <param name="url">
        /// Request path.
        /// <example>
        /// <code>
        /// Application
        ///     .Http.Get
        ///     .Url("/api/employee/all")
        ///     .Send()
        /// </code>
        /// </example> 
        /// </param>
        /// <returns></returns>
        public HttpModule Url(string url)
        {
            ConfigureHttpContext = x => x.Request.Path = url;
            return this;
        }

        /// <summary>
        /// Sets JWT token header for authentication.
        /// </summary>
        /// <param name="jwt">JWT token</param>
        /// <returns></returns>
        public HttpModule WithBearerToken(string jwt)
        {
            ConfigureHttpContext = x => x.Request.Headers["Authorization"] = $"Bearer {jwt}";
            return this;
        }
        
        /// <summary>
        /// Adds HTTP request header.
        /// </summary>
        /// <param name="key">Header key</param>
        /// <param name="value">Header value</param>
        /// <returns></returns>
        public HttpModule WithHeader(string key, string value)
        {
            ConfigureHttpContext = x => x.Request.Headers.Add(key, value);
            return this;
        }

        /// <summary>
        /// Sets the Accept HTTP header.
        /// </summary>
        /// <param name="accepts"></param>
        /// <returns></returns>
        public HttpModule Accepts(string accepts)
        {
            ConfigureHttpContext = x => x.Request.Headers.Accept = accepts;
            return this;
        }

        /// <summary>
        /// Append the given query key and value to the URI.
        /// </summary>
        /// <param name="key">The name of the query key.</param>
        /// <param name="value">The query value</param>
        /// <returns></returns>
        public HttpModule AddQuery(string key, string value)
        {
            ConfigureHttpContext = context =>
            {
                context.Request.QueryString = context.Request.QueryString.Add(key, value);
                context.Request.Query =
                    new QueryCollection(QueryHelpers.ParseQuery(context.Request.QueryString.Value!));
            };
            return this;
        }
        
        /// <summary>
        /// Append the given query key and value to the URI.
        /// </summary>
        /// <param name="key">The name of the query key.</param>
        /// <param name="value">The query value</param>
        /// <returns></returns>
        public HttpModule AddQuery(string key, object value)
        {
            return AddQuery(key, value.ToString());
        }

        /// <summary>
        /// Serializes an object and appends JSON to the request body
        /// </summary>
        /// <param name="body">The object to be serialized</param>
        /// <returns></returns>
        public HttpModule Json(object body)
        {
            Application.HttpService.AddJson(body);
            return this;
        }
        
        /// <summary>
        /// Serializes an object and appends XML to the request body
        /// </summary>
        /// <param name="body">The object to be serialized</param>
        /// <returns></returns>
        public HttpModule Xml(object body)
        {
            Application.HttpService.AddXml(body);
            return this;
        }

        /// <summary>
        /// Appends text to the request body
        /// </summary>
        /// <param name="text">The text to be added to the body</param>
        /// <returns></returns>
        public HttpModule Text(string text)
        {
            Application.HttpService.AddText(text);
            return this;
        }
        
        public HttpResponseModule HttpResponse => new(Application);
    }
}