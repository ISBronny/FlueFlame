using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using FlueFlame.Core.Serialization;
using FlueFlame.Http.Host;
using FlueFlame.Http.Modules;
using FluentAssertions;
using Moq;
using Moq.Protected;
using Tests.Unit.Core.Entities;

namespace Tests.Unit.FlueFlame.Http;

public class HttpModuleTests
{
	private const string BaseAddress = "http://test.com";

	private Mock<IFlueFlameHttpHost> GetHostMock(Action<HttpRequestMessage, CancellationToken> requestCallback = null, Action<HttpClient> configureClient = null)
	{
		requestCallback ??= (_, _) => { };
		
		var httpMessageHandlerMock = new Mock<HttpMessageHandler>();

		httpMessageHandlerMock
			.Protected()
			.Setup<Task<HttpResponseMessage>>(
				"SendAsync",
				ItExpr.IsAny<HttpRequestMessage>(),
				ItExpr.IsAny<CancellationToken>())
			.Callback(requestCallback)
			.ReturnsAsync(new HttpResponseMessage());
		
		var httpClient = new HttpClient(httpMessageHandlerMock.Object);
		httpClient.BaseAddress = new Uri(BaseAddress);
		configureClient?.Invoke(httpClient);

		var hostMock = new Mock<IFlueFlameHttpHost>();
		hostMock.SetupGet(x => x.HttpClient).Returns(httpClient);

		return hostMock;
	}
	
	[Theory]
	[InlineData("/api/tests")]
	[InlineData("api/tests")]
	public void Url_ValidUrl(string path)
	{
		var host = GetHostMock((request, _) =>
			request.RequestUri?.PathAndQuery.Should().Be("/api/tests"));

		var module = new HttpModule(host.Object, HttpMethod.Get);
		
		module.Url(path);
		module.Send();
	}
	
	[Fact]
	public void Url_NullUrl_Throws()
	{
		var host = GetHostMock();
		
		var module = new HttpModule(host.Object, HttpMethod.Get);

		FluentActions.Invoking(() => module.Url(null)).Should().Throw<ArgumentNullException>();
	}

	[Fact]
	public void WithBearerToken_ValidToken()
	{
		var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.e30.Et9HFtf9R3GEMA0IICOfFMVXY7kkTX1wr4qCyhIf58U";
		
		var host = GetHostMock((request, _) =>
			request.Headers.Authorization?.ToString().Should().Be($"Bearer {token}"));

		var module = new HttpModule(host.Object, HttpMethod.Get);
		
		module.WithJwtToken(token);
		
		module.Send();
	}
	
	[Fact]
	public void WithBearerToken_OverridesDefaultAuthHeader()
	{
		var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.e30.Et9HFtf9R3GEMA0IICOfFMVXY7kkTX1wr4qCyhIf58U";

		var host = GetHostMock((request, _) =>
				request.Headers.Authorization?.ToString().Should().Be($"Bearer {token}"),
			client =>
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer", "adsuhudi"));
		
		var module = new HttpModule(host.Object, HttpMethod.Get);

		module.WithJwtToken(token);
		
		module.Send();
	}

	[Theory]
	[InlineData("Accept-Language", "de-CH")]
	[InlineData("Access-Control-Allow-Credentials", "true")]
	public void WithHeader_SingleValue(string key, string value)
	{
		var host = GetHostMock((message, _) => message.Headers.GetValues(key).Should().Contain(value));

		var module = new HttpModule(host.Object, HttpMethod.Get);
		
		module.WithHeader(key, value);
		
		module.Send();
	}
	
	[Theory]
	[InlineData("Accept-Language", "en-US,en; q=0.5")]
	[InlineData("Transfer-Encoding", "gzip, chunked")]
	public void WithHeader_MultipleValuesSeparatedByComma(string key, string value)
	{
		var host = GetHostMock((message, _) => 
			message.Headers.GetValues(key).Should().BeEquivalentTo(value.Split(',', StringSplitOptions.TrimEntries)));

		var module = new HttpModule(host.Object, HttpMethod.Get);
		
		module.WithHeader(key, value);
		
		module.Send();
	}
	
	[Theory]
	[InlineData("Accept-Language", new[] {"en-US", "en; q=0.5"})]
	[InlineData("Transfer-Encoding", new[] {"gzip", "chunked"})]
	public void WithHeader_MultipleValues(string key, IEnumerable<string> values)
	{
		var host = GetHostMock((message, _) => 
			message.Headers.GetValues(key).Should().BeEquivalentTo(values));

		var module = new HttpModule(host.Object, HttpMethod.Get);
		
		module.WithHeader(key, values);
		
		module.Send();
	}

	[Fact]
	public void WithHeader_AddMultipleTimes()
	{
		var key = "my-key";
		var values = new[] { "one", "two", "three" };
		
		var host = GetHostMock((message, _) => 
			message.Headers.GetValues(key).Should().BeEquivalentTo(values));

		var module = new HttpModule(host.Object, HttpMethod.Get);

		foreach (var value in values)
		{
			module.WithHeader(key, value);
		}

		module.Send();
	}
	
	[Theory]
	[InlineData("text/html")]
	public void Accept_SingleValue(string accept)
	{
		var host = GetHostMock((message, _) => 
			message.Headers.Accept.Should().Contain(x=>x.MediaType == accept));

		var module = new HttpModule(host.Object, HttpMethod.Get);
		
		module.Accept(accept);
		
		module.Send();
	}
	
	[Theory]
	[InlineData("text/html, application/xhtml+xml")]
	[InlineData("text/html, application/xhtml+xml, application/xml, image/webp, */*")]
	public void Accept_MultipleValuesSeparatedByComma(string accept)
	{
		var host = GetHostMock((message, _) => 
			message.Headers.Accept.Select(x=>x.ToString()).Should().BeEquivalentTo(accept.Split(',', StringSplitOptions.TrimEntries)));

		var module = new HttpModule(host.Object, HttpMethod.Get);
		
		module.Accept(accept);
		
		module.Send();
	}
	
	[Theory]
	[InlineData("text/html", "application/xhtml+xml")]
	[InlineData("text/html", "application/xhtml+xml", "application/xml", "image/webp", "*/*")]
	public void Accept_MultipleValues(params string[] accept)
	{
		var host = GetHostMock((message, _) => 
			message.Headers.Accept.Select(x=>x.ToString()).Should().BeEquivalentTo(accept));

		var module = new HttpModule(host.Object, HttpMethod.Get);
		
		module.Accept(accept);
		
		module.Send();
	}

	[Fact]
	public void Accept_AddMultipleTimes()
	{
		var values = new[] { "text/html", "application/xhtml+xml", "application/xml" };
		
		var host = GetHostMock((message, _) => 
			message.Headers.Accept.Select(x=>x.ToString()).Should().BeEquivalentTo(values));

		var module = new HttpModule(host.Object, HttpMethod.Get);

		foreach (var value in values)
		{
			module.Accept(value);
		}

		module.Send();
	}
	
	[Fact]
	public void AddQuery_SingleParameter()
	{
		var host = GetHostMock((message, _) => 
			message.RequestUri?.Query.ToString().Should().Be("?age=18"));

		var module = new HttpModule(host.Object, HttpMethod.Get);
		
		module.AddQuery("age", 18);
		module.Send();
	}
	
	[Fact]
	public void AddQuery_MultiplyParameters()
	{
		var host = GetHostMock((message, _) => 
			message.RequestUri?.Query.ToString().Should().Be("?age=18&city=London"));

		var module = new HttpModule(host.Object, HttpMethod.Get);
		
		module.AddQuery("age", 18);
		module.AddQuery("city", "London");
		module.Send();
	}
	
	[Fact]
	public void Json_TestEntity()
	{
		var entity = new TestEntity()
		{
			StringProperty = "String",
			EnumProperty = TestEnum.Value2,
			IntProperty = 123,
			DateTimeProperty = DateTime.Parse("1992-04-05")
		};
		
		var host = GetHostMock((message, _) =>
		{
			message.Content.Headers.ContentType.MediaType.Should().Be("application/json");
			message.Content.Headers.ContentType.CharSet.Should().Be("utf-8");
			message.Content.ReadFromJsonAsync<TestEntity>(cancellationToken: _).Result
				.Should().BeEquivalentTo(entity);
		});

		host.SetupGet(x => x.JsonSerializer)
			.Returns(new TextJsonSerializer());

		var module = new HttpModule(host.Object, HttpMethod.Get);
		
		module.Json(entity);
		module.Send();
	}
	
	[Fact]
	public void Xml_TestEntity()
	{
		var entity = new TestEntity()
		{
			StringProperty = "String",
			EnumProperty = TestEnum.Value2,
			IntProperty = 123,
			DateTimeProperty = DateTime.Parse("1992-04-05")
		};
		
		var host = GetHostMock((message, _) =>
		{
			message.Content.Headers.ContentType.MediaType.Should().Be("text/xml");
			message.Content.Headers.ContentType.CharSet.Should().Be("utf-8");
			new XmlSerializer().DeserializeObject<TestEntity>(message.Content.ReadAsStringAsync(_).Result)
				.Should().BeEquivalentTo(entity);
		});

		host.SetupGet(x => x.XmlSerializer)
			.Returns(new XmlSerializer());

		var module = new HttpModule(host.Object, HttpMethod.Get);
		
		module.Xml(entity);
		module.Send();
	}
	
	[Fact]
	public void Text()
	{
		var text = "MyText";

		var host = GetHostMock((message, _) =>
		{
			message.Content.Headers.ContentType.MediaType.Should().Be("text/plain");
			message.Content.Headers.ContentType.CharSet.Should().Be("utf-8");
			Encoding.UTF8.GetString(message.Content.ReadAsByteArrayAsync(_).Result).Should().Be(text);
		});

		host.SetupGet(x => x.XmlSerializer)
			.Returns(new XmlSerializer());

		var module = new HttpModule(host.Object, HttpMethod.Get);
		
		module.Text(text);
		module.Send();
	}
}