using System.Net;

namespace Testing.Tests.AspNet.NUnit.Assertions;

public class HeadersAssertionsTests : TestBase
{
    [Test]
    public void AssertCustomHeader()
    {
        var header = new { Key = "my-custom-header", Value = "my-custom-value" };
        Application
            .Http.Get
            .Url("/api/headers/header")
            .QueryParam("key", header.Key)
            .QueryParam("value", header.Value)
            .Send()
            .HttpResponse
                .AssertStatusCode(HttpStatusCode.OK)
                .AssertContainsHeaders(header.Key)
                .AssertHeader(header.Key, header.Value)
                .AssertHeaderPattern(header.Key, @"my-*-va?ue");
    }
    
    [Test]
    public void AssertDoesNotContainsHeaders()
    {
        Application
            .Http.Get
            .Url("/api/employee/all")
            .Send()
            .HttpResponse
                .AssertStatusCode(HttpStatusCode.OK)
                .AssertDoesNotContainsHeaders("Strict-Transport-Security", "X-Powered-By");
    }
    
    [Test]
    public void AssertContentTypeHeader()
    {
        Application
            .Http.Get
            .Url("/api/employee/all")
            .Send()
            .HttpResponse
                .AssertStatusCode(HttpStatusCode.OK)
                .AssertHeader("Content-Type", "application/json; charset=utf-8")
                .AssertBodyLength(338);
    }
}