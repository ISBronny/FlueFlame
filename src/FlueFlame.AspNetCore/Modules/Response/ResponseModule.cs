using System.Collections.Generic;
using System.Linq;
using System.Net;
using FlueFlame.AspNetCore.Common;
using FlueFlame.AspNetCore.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;


namespace FlueFlame.AspNetCore.Modules.Response
{
    public class ResponseModule : AspNetModuleBase
    {
        protected HttpResponse HttpResponse { get; }
        protected HttpResponseBodyHelper BodyHelper { get; }
        public ResponseModule(FlueFlameHost application) : base(application)
        {
            HttpResponse = Application.HttpContext.Response;
            BodyHelper = new HttpResponseBodyHelper(HttpResponse);
        }

        public JsonResponseModule AsJson => new JsonResponseModule(Application);
        public XmlResponseModule AsXml => new XmlResponseModule(Application);
        public TextResponseModule AsText => new TextResponseModule(Application);
        
        
        #region Status Code
    
        public ResponseModule AssertStatusCode(HttpStatusCode statusCode)
        {
            AssertStatusCode((int)statusCode);
            return this;
        }
    
        public ResponseModule AssertStatusCode(int statusCode)
        {
            statusCode.Should().Be(HttpResponse.StatusCode);
            return this;
        }
    
        #endregion
    
        #region Headers

        public ResponseModule AssertContainsHeaders(params string[] headers)
        {
            HttpResponse.Headers.Select(x => x.Key).Should().Contain(headers);
            return this;
        }
    
        public ResponseModule AssertDoesNotContainsHeaders(params string[] headers)
        {
            HttpResponse.Headers.Select(x=>x.Key).Should().NotContain(headers);
            return this;
        }

        public ResponseModule AssertHeader(string key, string value)
        {
            HttpResponse.Headers.Should().Contain(x => x.Key.Equals(key));
            HttpResponse.Headers[key].Should().Contain(value);
            return this;
        }
    
        public ResponseModule AssertHeaderPattern(string key, string pattern)
        {
            HttpResponse.Headers.Should().Contain(x => x.Key.Equals(key));
            HttpResponse.Headers[key].Should().ContainMatch(pattern);
            return this;
        }

        #endregion
    
        public ResponseModule AssertBodyLength(long length)
        {
            HttpResponse.Body.Length.Should().Be(length);
            return this;
        }
    }
}