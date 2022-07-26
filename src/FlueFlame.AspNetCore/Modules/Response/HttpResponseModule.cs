﻿using System.Linq;
using System.Net;
using FlueFlame.AspNetCore.Deserialization;
using FlueFlame.AspNetCore.Modules.Response.Content;
using FlueFlame.AspNetCore.Modules.Response.Content.Formatted;
using FlueFlame.AspNetCore.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;


namespace FlueFlame.AspNetCore.Modules.Response
{
    public sealed class HttpResponseModule : ResponseModule
    {
        private HttpResponse HttpResponse { get; }
        private HttpResponseBodyHelper BodyHelper { get; }
        internal HttpResponseModule(IFlueFlameHost application, HttpResponse httpResponse) : base(application)
        {
            HttpResponse = httpResponse;
            BodyHelper = new HttpResponseBodyHelper(HttpResponse);
        }


        #region Status Code
    
        /// <summary>
        /// Asserts that the response Status Code exactly the same as expected value.
        /// </summary>
        /// <param name="statusCode">Expected Status Code</param>
        /// <returns></returns>
        public HttpResponseModule AssertStatusCode(HttpStatusCode statusCode)
        {
            AssertStatusCode((int)statusCode);
            return this;
        }
    
        /// <summary>
        /// Asserts that the response Status Code exactly the same as expected value.
        /// </summary>
        /// <param name="statusCode">Expected Status Code</param>
        /// <returns></returns>
        public HttpResponseModule AssertStatusCode(int statusCode)
        {
            statusCode.Should().Be(HttpResponse.StatusCode);
            return this;
        }
    
        #endregion
    
        #region Headers

        /// <summary>
        /// Expects the response headers to contain the specified headers.
        /// </summary>
        /// <param name="headers">Expected headers</param>
        /// <returns></returns>
        public HttpResponseModule AssertContainsHeaders(params string[] headers)
        {
            HttpResponse.Headers.Select(x => x.Key).Should().Contain(headers);
            return this;
        }
    
        /// <summary>
        /// Asserts that the response headers does not contain the specified headers.
        /// </summary>
        /// <param name="headers">Unexpected headers</param>
        /// <returns></returns>
        public HttpResponseModule AssertDoesNotContainsHeaders(params string[] headers)
        {
            HttpResponse.Headers.Select(x=>x.Key).Should().NotContain(headers);
            return this;
        }

        /// <summary>
        /// Asserts that the response has a header with the specified value.
        /// </summary>
        /// <param name="key">Header key</param>
        /// <param name="value">Header value</param>
        /// <returns></returns>
        public HttpResponseModule AssertHeader(string key, string value)
        {
            HttpResponse.Headers.Should().Contain(x => x.Key.Equals(key));
            HttpResponse.Headers[key].Should().Contain(value);
            return this;
        }
    
        /// <summary>
        /// Asserts that the header value matches the wildcard pattern.
        /// </summary>
        /// <param name="key">Header key</param>
        /// <param name="pattern">
        /// The pattern to match against the subject.
        /// This parameter can contain a combination of literal text and wildcard (* and ?) characters,
        /// but it doesn't support regular expressions.
        /// </param>
        /// <returns></returns>
        public HttpResponseModule AssertHeaderPattern(string key, string pattern)
        {
            HttpResponse.Headers.Should().Contain(x => x.Key.Equals(key));
            HttpResponse.Headers[key].Should().ContainMatch(pattern);
            return this;
        }

        #endregion
        

        /// <summary>
        /// Returns the module to work with the response body as JSON.
        /// </summary>
        public override JsonContentResponseModule AsJson => new(Application, Application.ServiceFactory.Get<IJsonSerializer>(),BodyHelper.ReadAsText());
        /// <summary>
        /// Returns the module to work with the response body as XML.
        /// </summary>
        public override XmlContentResponseModule AsXml => new(Application, Application.ServiceFactory.Get<IXmlSerializer>(), BodyHelper.ReadAsText());
        /// <summary>
        /// Returns the module to work with the response body as text.
        /// </summary>
        public override TextContentResponseModule AsText => new(Application, BodyHelper.ReadAsText());
    }
}