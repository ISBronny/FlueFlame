using System.Net;
using FlueFlame.Http.Modules;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Moq;

namespace Testing.Tests.UnitTests.Response;

public class HttpResponseTests : TestBase
{

	// [Fact]
	// public void AssertStatusCode_ExpectedStatusCode_ShouldNotThrow()
	// {
	// 	var statusCode = HttpStatusCode.OK;
	// 	
	// 	var httpResponse = GetHttpResponse(statusCode);
	// 	
	// 	var module = new HttpResponseModule(FlueFlameHost, httpResponse);
	//
	// 	FluentActions.Invoking(() => module.AssertStatusCode(statusCode))
	// 		.Should().NotThrow();
	// }
	//
	// [Fact]
	// public void AssertStatusCode_UnexpectedStatusCode_ShouldThrow()
	// {
	// 	var httpResponse = GetHttpResponse();
	//
	// 	var module = new HttpResponseModule(FlueFlameHost, httpResponse);
	//
	// 	FluentActions.Invoking(() => module.AssertStatusCode(HttpStatusCode.NotFound))
	// 		.Should().Throw<Exception>();
	// }
	//
	// [Fact]
	// public void AssertHeader_ExpectedHeader_ShouldNotThrow()
	// {
	// 	var headerKey = "MyKey";
	// 	var headerValues = new[] { "value1", "value2" };
	// 	
	// 	var header = new Dictionary<string, StringValues> { { headerKey, new StringValues(headerValues) } };
	//
	// 	var httpResponse = GetHttpResponse(headers: header);
	// 	
	// 	var module = new HttpResponseModule(FlueFlameHost, httpResponse);
	//
	// 	FluentActions.Invoking(() => module.AssertHeader(headerKey, headerValues[1]))
	// 		.Should().NotThrow();
	// }
	//
	// [Fact]
	// public void AssertHeader_UnexpectedHeaderValue_ShouldThrow()
	// {
	// 	var headerKey = "MyKey";
	// 	var headerValues = new[] { "value1", "value2" };
	// 	
	// 	var header = new Dictionary<string, StringValues> { { headerKey, new StringValues(headerValues) } };
	//
	// 	var httpResponse = GetHttpResponse(headers: header);
	// 	
	// 	var module = new HttpResponseModule(FlueFlameHost, httpResponse);
	//
	// 	FluentActions.Invoking(() => module.AssertHeader(headerKey, "value3"))
	// 		.Should().Throw<Exception>();
	// }
	//
	// [Fact]
	// public void AssertHeader_UnexpectedHeaderKey_ShouldThrow()
	// {
	// 	var headerKey = "MyKey";
	// 	var headerValues = new[] { "value1", "value2" };
	// 	
	// 	var header = new Dictionary<string, StringValues> { { headerKey, new StringValues(headerValues) } };
	//
	// 	var httpResponse = GetHttpResponse(headers: header);
	// 	
	// 	var module = new HttpResponseModule(FlueFlameHost, httpResponse);
	//
	// 	FluentActions.Invoking(() => module.AssertHeader("SomeKey", headerValues[0]))
	// 		.Should().Throw<Exception>();
	// }
	//
	// [Fact]
	// public void AssertContainsHeaders_ExpectedHeaders_ShouldNotThrow()
	// {
	// 	var headerKeys = new[] {"MyKey1", "MyKey2", "MyKey3"};
	//
	// 	var header =
	// 		new Dictionary<string, StringValues>(headerKeys.Select(x =>
	// 			new KeyValuePair<string, StringValues>(x, new StringValues())));
	//
	// 	var httpResponse = GetHttpResponse(headers: header);
	// 	
	// 	var module = new HttpResponseModule(FlueFlameHost, httpResponse);
	//
	// 	FluentActions.Invoking(() => module.AssertContainsHeaders(headerKeys))
	// 		.Should().NotThrow();
	// }
	//
	// [Fact]
	// public void AssertContainsHeaders_HasNoRequiredHeader_ShouldThrow()
	// {
	// 	var headerKeys = new[] {"MyKey1", "MyKey2"};
	//
	// 	var header =
	// 		new Dictionary<string, StringValues>(headerKeys.Select(x =>
	// 			new KeyValuePair<string, StringValues>(x, new StringValues())));
	//
	// 	var httpResponse = GetHttpResponse(headers: header);
	// 	
	// 	var module = new HttpResponseModule(FlueFlameHost, httpResponse);
	//
	// 	FluentActions.Invoking(() => module.AssertContainsHeaders("MyKey1", "MyKey2", "MyKey3"))
	// 		.Should().Throw<Exception>();
	// }
	//
	// [Fact]
	// public void AssertDoesNotContainsHeaders_HasNoUnexpectedHeaders_ShouldNotThrow()
	// {
	// 	var headerKeys = new[] {"MyKey1", "MyKey2"};
	//
	// 	var header =
	// 		new Dictionary<string, StringValues>(headerKeys.Select(x =>
	// 			new KeyValuePair<string, StringValues>(x, new StringValues())));
	//
	// 	var httpResponse = GetHttpResponse(headers: header);
	// 	
	// 	var module = new HttpResponseModule(FlueFlameHost, httpResponse);
	//
	// 	FluentActions.Invoking(() => module.AssertDoesNotContainsHeaders("LolKey", "MyKey3"))
	// 		.Should().NotThrow();
	// }
	//
	// [Fact]
	// public void AssertDoesNotContainsHeaders_HasUnexpectedHeaders_ShouldThrow()
	// {
	// 	var headerKeys = new[] {"MyKey1", "MyKey2"};
	//
	// 	var header =
	// 		new Dictionary<string, StringValues>(headerKeys.Select(x =>
	// 			new KeyValuePair<string, StringValues>(x, new StringValues())));
	//
	// 	var httpResponse = GetHttpResponse(headers: header);
	// 	
	// 	var module = new HttpResponseModule(FlueFlameHost, httpResponse);
	//
	// 	FluentActions.Invoking(() => module.AssertDoesNotContainsHeaders("MyKey1", "MyKey3"))
	// 		.Should().Throw<Exception>();
	// }
	//
	// [Fact]
	// public void AssertHeaderPattern_HasExpectedHeaderValue_ShouldNotThrow()
	// {
	// 	var headerKey = "MyKey";
	// 	var headerValues = new[] { "value1", "value2", "someVal"};
	// 	
	// 	var header = new Dictionary<string, StringValues> { { headerKey, new StringValues(headerValues) } };
	//
	// 	var httpResponse = GetHttpResponse(headers: header);
	// 	
	// 	var module = new HttpResponseModule(FlueFlameHost, httpResponse);
	//
	// 	FluentActions.Invoking(() => module.AssertHeaderPattern(headerKey, "value?"))
	// 		.Should().NotThrow();
	// }
	//
	// [Fact]
	// public void AssertHeaderPattern_HasNoExpectedHeaderValue_ShouldThrow()
	// {
	// 	var headerKey = "MyKey";
	// 	var headerValues = new[] { "hfg", "hrefg", "jytedh"};
	// 	
	// 	var header = new Dictionary<string, StringValues> { { headerKey, new StringValues(headerValues) } };
	//
	// 	var httpResponse = GetHttpResponse(headers: header);
	// 	
	// 	var module = new HttpResponseModule(FlueFlameHost, httpResponse);
	//
	// 	FluentActions.Invoking(() => module.AssertHeaderPattern(headerKey, "value?"))
	// 		.Should().Throw<Exception>();
	// }
	//
	// private HttpResponse GetHttpResponse(
	// 	HttpStatusCode statusCode = HttpStatusCode.OK,
	// 	Dictionary<string, StringValues> headers = null)
	// {
	// 	var httpResponseMock = new Mock<HttpResponse>();
	// 	
	// 	//Status Code
	// 	httpResponseMock.Setup(x => x.StatusCode).Returns(() => (int)statusCode);
	//
	// 	//Headers
	// 	headers ??= new Dictionary<string, StringValues>();
	// 	httpResponseMock.Setup(x => x.Headers).Returns(() => new HeaderDictionary(headers));
	// 	
	//
	// 	return httpResponseMock.Object;
	// }
}