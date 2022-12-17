using System.Net.Http.Headers;
using System.Text;
using System.Web;
using FlueFlame.Core;
using FlueFlame.Http.Host;

namespace FlueFlame.Http.Modules
{
    public class HttpModule : FlueFlameModuleBase<IFlueFlameHttpHost>
    {
        private HttpRequestMessage HttpRequestMessage { get; }
        private HttpResponseMessage HttpResponseMessage { get; set; }
        private UriBuilder UriBuilder { get; }

        internal HttpModule(
            IFlueFlameHttpHost application,
            HttpMethod httpMethod
            ) : base(application)
        {
            UriBuilder = new UriBuilder(Host.HttpClient.BaseAddress);
            HttpRequestMessage = new HttpRequestMessage();
            HttpRequestMessage.Method = httpMethod;

 
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
            UriBuilder.Path = url;
            return this;
        }

        /// <summary>
        /// Sets JWT token header for authentication.
        /// </summary>
        /// <param name="jwt">JWT token</param>
        /// <returns></returns>
        public HttpModule WithBearerToken(string jwt)
        {
            HttpRequestMessage.Headers.Add("Authorization",  $"Bearer {jwt}");
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
            HttpRequestMessage.Headers.Add(key,  value);
            return this;
        }

        /// <summary>
        /// Sets the Accept HTTP header.
        /// </summary>
        /// <param name="mediaType"></param>
        /// <returns></returns>
        public HttpModule Accepts(string mediaType)
        {
            HttpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));;
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
            var query = HttpUtility.ParseQueryString(UriBuilder.Query);
            query[key] = value;
            UriBuilder.Query = query.ToString();
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
        /// Sends an HTTP request.
        /// </summary>
        /// <returns></returns>
        public HttpModule Send()
        {
            HttpRequestMessage.RequestUri = UriBuilder.Uri;
            HttpResponseMessage = Host.HttpClient.SendAsync(HttpRequestMessage).Result;
            return this;
        }

        /// <summary>
        /// Serializes an object and appends JSON to the request body
        /// </summary>
        /// <param name="body">The object to be serialized</param>
        /// <returns></returns>
        public HttpModule Json(object body)
        {
            var json = Host.JsonSerializer.SerializeObject(body);

            HttpRequestMessage.Content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json");
            
            return this;
        }
        
        /// <summary>
        /// Serializes an object and appends XML to the request body
        /// </summary>
        /// <param name="body">The object to be serialized</param>
        /// <returns></returns>
        public HttpModule Xml(object body)
        {
            var xml = Host.XmlSerializer.SerializeObject(body);

            HttpRequestMessage.Content = new StringContent(
                xml,
                Encoding.UTF8,
                "text/xml");
            return this;
        }

        /// <summary>
        /// Appends text to the request body
        /// </summary>
        /// <param name="text">The text to be added to the body</param>
        /// <returns></returns>
        public HttpModule Text(string text)
        {
            HttpRequestMessage.Content = new StringContent(text, Encoding.UTF8);
            return this;
        }
        
        public HttpResponseModule Response => new(Host, HttpResponseMessage);
    }
}