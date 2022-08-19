using System.Linq;
using System.Net;
using FlueFlame.AspNetCore.Common;
using FlueFlame.AspNetCore.Modules.Response.Content;
using FlueFlame.AspNetCore.Modules.Response.Content.Formatted;
using FlueFlame.AspNetCore.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;


namespace FlueFlame.AspNetCore.Modules.Response
{
    public class HttpResponseModule : ResponseModule
    {
        protected HttpResponse HttpResponse { get; }
        protected HttpResponseBodyHelper BodyHelper { get; }
        public HttpResponseModule(FlueFlameHost application) : base(application)
        {
            HttpResponse = Application.HttpContext.Response;
            BodyHelper = new HttpResponseBodyHelper(HttpResponse);
        }


        #region Status Code
    
        public HttpResponseModule AssertStatusCode(HttpStatusCode statusCode)
        {
            AssertStatusCode((int)statusCode);
            return this;
        }
    
        public HttpResponseModule AssertStatusCode(int statusCode)
        {
            statusCode.Should().Be(HttpResponse.StatusCode);
            return this;
        }
    
        #endregion
    
        #region Headers

        public HttpResponseModule AssertContainsHeaders(params string[] headers)
        {
            HttpResponse.Headers.Select(x => x.Key).Should().Contain(headers);
            return this;
        }
    
        public HttpResponseModule AssertDoesNotContainsHeaders(params string[] headers)
        {
            HttpResponse.Headers.Select(x=>x.Key).Should().NotContain(headers);
            return this;
        }

        public HttpResponseModule AssertHeader(string key, string value)
        {
            HttpResponse.Headers.Should().Contain(x => x.Key.Equals(key));
            HttpResponse.Headers[key].Should().Contain(value);
            return this;
        }
    
        public HttpResponseModule AssertHeaderPattern(string key, string pattern)
        {
            HttpResponse.Headers.Should().Contain(x => x.Key.Equals(key));
            HttpResponse.Headers[key].Should().ContainMatch(pattern);
            return this;
        }

        #endregion
    
        public HttpResponseModule AssertBodyLength(long length)
        {
            HttpResponse.Body.Length.Should().Be(length);
            return this;
        }

        public override JsonContentResponseModule AsJson => new(Application, BodyHelper.ReadAsText());
        public override XmlContentResponseModule AsXml => new(Application, BodyHelper.ReadAsText());
        public override TextContentResponseModule AsText => new(Application, BodyHelper.ReadAsText());
    }
}