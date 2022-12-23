using System.Net;
using FlueFlame.Http.Host;
using FlueFlame.Http.Modules;
using FluentAssertions;
using Moq;
using Tests.Unit.Core;

namespace Tests.Unit.FlueFlame.Http;


public class HttpTestBase : TestBase
{
	protected IFlueFlameHttpHost FlueFlameHttpHost => new Mock<IFlueFlameHttpHost>().Object;
}
public class HttpResponseTests : HttpTestBase
{

	[Fact]
	public void AssertStatusCode_ExpectedStatusCode_ShouldNotThrow()
	{
		var statusCode = HttpStatusCode.OK;
		
		var httpResponse = GetHttpResponse(statusCode);
		
		var module = new HttpResponseModule(FlueFlameHttpHost, httpResponse);
	
		FluentActions.Invoking(() => module.AssertStatusCode(statusCode))
			.Should().NotThrow();
	}
	
	[Fact]
	public void AssertStatusCode_UnexpectedStatusCode_ShouldThrow()
	{
		var httpResponse = GetHttpResponse();
	
		var module = new HttpResponseModule(FlueFlameHttpHost, httpResponse);
	
		FluentActions.Invoking(() => module.AssertStatusCode(HttpStatusCode.NotFound))
			.Should().Throw<Exception>();
	}
	
	[Fact]
	public void AssertHeader_ExpectedHeader_ShouldNotThrow()
	{
		var headerKey = "MyKey";
		var headerValues = new[] { "value1", "value2" };
		
		var header = new Dictionary<string, IEnumerable<string>> { { headerKey, headerValues } };
	
		var httpResponse = GetHttpResponse(headers: header);
		
		var module = new HttpResponseModule(FlueFlameHttpHost, httpResponse);
	
		FluentActions.Invoking(() => module.AssertHeader(headerKey, headerValues[1]))
			.Should().NotThrow();
	}
	
	[Fact]
	public void AssertHeader_UnexpectedHeaderValue_ShouldThrow()
	{
		var headerKey = "MyKey";
		var headerValues = new[] { "value1", "value2" };
		
		var header = new Dictionary<string, IEnumerable<string>> { { headerKey, headerValues } };
	
		var httpResponse = GetHttpResponse(headers: header);
		
		var module = new HttpResponseModule(FlueFlameHttpHost, httpResponse);
	
		FluentActions.Invoking(() => module.AssertHeader(headerKey, "value3"))
			.Should().Throw<Exception>();
	}
	
	[Fact]
	public void AssertHeader_UnexpectedHeaderKey_ShouldThrow()
	{
		var headerKey = "MyKey";
		var headerValues = new[] { "value1", "value2" };
		
		var header = new Dictionary<string, IEnumerable<string>> { { headerKey, headerValues} };
	
		var httpResponse = GetHttpResponse(headers: header);
		
		var module = new HttpResponseModule(FlueFlameHttpHost, httpResponse);
	
		FluentActions.Invoking(() => module.AssertHeader("SomeKey", headerValues[0]))
			.Should().Throw<Exception>();
	}
	
	[Fact]
	public void AssertContainsHeaders_ExpectedHeaders_ShouldNotThrow()
	{
		var headerKeys = new[] {"MyKey1", "MyKey2", "MyKey3"};
	
		var header =
			new Dictionary<string, IEnumerable<string>>(headerKeys.Select(x =>
				new KeyValuePair<string, IEnumerable<string>>(x, new[] {"foo"})));
	
		var httpResponse = GetHttpResponse(headers: header);
		
		var module = new HttpResponseModule(FlueFlameHttpHost, httpResponse);
	
		FluentActions.Invoking(() => module.AssertContainsHeaders(headerKeys))
			.Should().NotThrow();
	}
	
	[Fact]
	public void AssertContainsHeaders_HasNoRequiredHeader_ShouldThrow()
	{
		var headerKeys = new[] {"MyKey1", "MyKey2"};
	
		var header =
			new Dictionary<string, IEnumerable<string>>(headerKeys.Select(x =>
				new KeyValuePair<string, IEnumerable<string>>(x, new[] {"foo"})));
	
		var httpResponse = GetHttpResponse(headers: header);
		
		var module = new HttpResponseModule(FlueFlameHttpHost, httpResponse);
	
		FluentActions.Invoking(() => module.AssertContainsHeaders("MyKey1", "MyKey2", "MyKey3"))
			.Should().Throw<Exception>();
	}
	
	[Fact]
	public void AssertDoesNotContainsHeaders_HasNoUnexpectedHeaders_ShouldNotThrow()
	{
		var headerKeys = new[] {"MyKey1", "MyKey2"};
	
		var header =
			new Dictionary<string, IEnumerable<string>>(headerKeys.Select(x =>
				new KeyValuePair<string, IEnumerable<string>>(x, new[] {"foo"})));
	
		var httpResponse = GetHttpResponse(headers: header);
		
		var module = new HttpResponseModule(FlueFlameHttpHost, httpResponse);
	
		FluentActions.Invoking(() => module.AssertDoesNotContainsHeaders("LolKey", "MyKey3"))
			.Should().NotThrow();
	}
	
	[Fact]
	public void AssertDoesNotContainsHeaders_HasUnexpectedHeaders_ShouldThrow()
	{
		var headerKeys = new[] {"MyKey1", "MyKey2"};
	
		var header =
			new Dictionary<string, IEnumerable<string>>(headerKeys.Select(x =>
				new KeyValuePair<string, IEnumerable<string>>(x, new[] {"foo"})));
	
		var httpResponse = GetHttpResponse(headers: header);
		
		var module = new HttpResponseModule(FlueFlameHttpHost, httpResponse);
	
		FluentActions.Invoking(() => module.AssertDoesNotContainsHeaders("MyKey1", "MyKey3"))
			.Should().Throw<Exception>();
	}
	
	[Fact]
	public void AssertHeaderPattern_HasExpectedHeaderValue_ShouldNotThrow()
	{
		var headerKey = "MyKey";
		var headerValues = new[] { "value1", "value2", "someVal"};
		
		var header = new Dictionary<string, IEnumerable<string>> { { headerKey,headerValues } };
	
		var httpResponse = GetHttpResponse(headers: header);
		
		var module = new HttpResponseModule(FlueFlameHttpHost, httpResponse);
	
		FluentActions.Invoking(() => module.AssertHeaderPattern(headerKey, "value?"))
			.Should().NotThrow();
	}
	
	[Fact]
	public void AssertHeaderPattern_HasNoExpectedHeaderValue_ShouldThrow()
	{
		var headerKey = "MyKey";
		var headerValues = new[] { "hfg", "hrefg", "jytedh"};
		
		var header = new Dictionary<string, IEnumerable<string>> { { headerKey, headerValues } };
	
		var httpResponse = GetHttpResponse(headers: header);
		
		var module = new HttpResponseModule(FlueFlameHttpHost, httpResponse);
	
		FluentActions.Invoking(() => module.AssertHeaderPattern(headerKey, "value?"))
			.Should().Throw<Exception>();
	}
	
	[Fact]
	public void AssertStatusCode()
	{
		var statusCode = HttpStatusCode.Conflict;
		
		var httpResponse = GetHttpResponse(statusCode);
		
		var module = new HttpResponseModule(FlueFlameHttpHost, httpResponse);
	
		FluentActions.Invoking(() => module.AssertStatusCode(statusCode))
			.Should().NotThrow();
	}
	
	[Fact]
	public void AssertStatusCode_Number()
	{
		var statusCode = 404;
		
		var httpResponse = GetHttpResponse(HttpStatusCode.NotFound);
		
		var module = new HttpResponseModule(FlueFlameHttpHost, httpResponse);
	
		FluentActions.Invoking(() => module.AssertStatusCode(statusCode))
			.Should().NotThrow();
	}
	
	[Fact]
	public void AssertRawResponse()
	{
		var httpResponse = GetHttpResponse(HttpStatusCode.NotFound);
		
		var module = new HttpResponseModule(FlueFlameHttpHost, httpResponse);
	
		FluentActions.Invoking(() => module.AssertRawResponse(message =>
			{
				message.StatusCode.Should().Be(HttpStatusCode.Accepted);
			}))
			.Should().Throw<Exception>();
	}
	
	private HttpResponseMessage GetHttpResponse(
		HttpStatusCode statusCode = HttpStatusCode.OK,
		Dictionary<string, IEnumerable<string>> headers = null)
	{
		var response = new HttpResponseMessage(statusCode);

		if(headers!=null)
			foreach (var header in headers)
				response.Headers.Add(header.Key, header.Value.ToArray());
		
		return response;
	}
}