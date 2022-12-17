using FlueFlame.Core.Serialization;
using Moq;

namespace Testing.Tests.UnitTests.Http;

public class HttpFacadeTests : TestBase
{
	private IJsonSerializer JsonSerializer => new Mock<IJsonSerializer>().Object;
	private IXmlSerializer XmlSerializer => new Mock<IXmlSerializer>().Object;
	
	// private readonly HttpService _httpService;
	// private readonly HttpContext _httpContext;
	//
	// private static string Key = "MyKey";
	// private static string SingleValue = "MyValue";
	// private static StringValues MultipleValues = new[] {"MyValue1", "MyValue2", "MyValue3"};
	//
	// private static string Token =
	// 	"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c"; 
	//
	//
	// public HttpFacadeTests()
	// {
	// 	_httpService = new HttpService(JsonSerializer, XmlSerializer);
	// 	_httpFacade = new HttpFacade(FlueFlameHost, _httpService);
	// 	_httpContext = new DefaultHttpContext();
	// }
	//
	// [Fact]
	// public void AddDefaultHeader_SingleValue_Added()
	// {
	// 	_httpFacade.AddDefaultHeader(Key, SingleValue);
	// 	_httpService.SetupHttpContext(_httpContext);
	// 	Assert.Contains(new KeyValuePair<string, StringValues>(Key, SingleValue), _httpContext.Request.Headers);
	// }
	//
	// [Fact]
	// public void AddDefaultHeader_SingleDuplicate_Overwritten()
	// {
	// 	_httpFacade.AddDefaultHeader(Key, SingleValue + "abracadabra");
	// 	_httpFacade.AddDefaultHeader(Key, SingleValue);
	// 	_httpService.SetupHttpContext(_httpContext);
	// 	Assert.Contains(new KeyValuePair<string, StringValues>(Key, SingleValue), _httpContext.Request.Headers);
	// }
	//
	// [Fact]
	// public void AddDefaultHeader_MultipleValues_Added()
	// {
	// 	_httpFacade.AddDefaultHeader(Key, MultipleValues);
	// 	_httpService.SetupHttpContext(_httpContext);
	// 	Assert.Contains(new KeyValuePair<string, StringValues>(Key, MultipleValues), _httpContext.Request.Headers);
	// }
	//
	// [Fact]
	// public void AddDefaultHeader_Reseted_DoesNotExists()
	// {
	// 	_httpFacade.AddDefaultHeader(Key, MultipleValues);
	// 	_httpFacade.Reset();
	// 	_httpService.SetupHttpContext(_httpContext);
	// 	Assert.DoesNotContain(new KeyValuePair<string, StringValues>(Key, MultipleValues), _httpContext.Request.Headers);
	// }
	//
	// [Fact]
	// public void AddDefaultBearerToken_JustToken_Added()
	// {
	// 	_httpFacade.AddDefaultBearerToken(Token);
	// 	_httpService.SetupHttpContext(_httpContext);
	// 	Assert.Equal($"Bearer {Token}", _httpContext.Request.Headers.Authorization);
	// }
	//
	// [Fact]
	// public void AddDefaultBearerToken_TokenWithBearerKeyword_Added()
	// {
	// 	_httpFacade.AddDefaultBearerToken($"Bearer {Token}");
	// 	_httpService.SetupHttpContext(_httpContext);
	// 	Assert.Equal($"Bearer {Token}", _httpContext.Request.Headers.Authorization);
	// }
}