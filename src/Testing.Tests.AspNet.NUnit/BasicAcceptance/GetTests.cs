using System.Net;
using FluentAssertions;
using Testing.TestData.AspNetCore.Models;

namespace Testing.Tests.AspNet.NUnit.BasicAcceptance;

public class GetTests : TestBase
{
    [Test]
    public void GetReturnsOk()
    {
        Application
            .Http.Get
            .Url("/api/employee/all")
            .Send()
            .HttpResponse
                .AssertStatusCode(HttpStatusCode.OK);
    }
    
    [Test]
    public void GetWithQueryReturnsOk()
    {
        Application
            .Http.Get
            .Url("/api/employee/older-than")
            .QueryParam("olderThan", 45)
            .Send()
            .HttpResponse
                .AssertStatusCode(HttpStatusCode.OK)
                .AsJson
                    .AssertThat<Employee[]>(employees => employees.Should().NotContain(x=>x.Age<45));
    }
    
    [Test]
    public void GetWithQueryReturnsBadRequest()
    {
        Application
            .Http.Get
            .Url("/api/employee/older-than")
            .QueryParam("olderThan", -34)
            .Send()
            .HttpResponse
                .AssertStatusCode(HttpStatusCode.BadRequest);
    }

    [Test]
    public void GetReturnsNotFound()
    {
        Application
            .Http.Get
            .Url("/api/notfound")
            .Send()
            .HttpResponse
                .AssertStatusCode(HttpStatusCode.NotFound);
    }
}