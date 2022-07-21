using System.Net;
using FlueFlame.Extensions.Assertions.NUnit;
using Testing.TestData.AspNetCore.Models;

namespace Testsing.Tests.AspNet.NUnit.BasicAcceptance;

public class GetTests : TestBase
{
    [Test]
    public void GetReturnsOk()
    {
        Application
            .Http.Get
            .Url("/api/employee/all")
            .Send()
            .Response
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
            .Response
                .AssertStatusCode(HttpStatusCode.OK)
                .AsJson.AssertThat<Employee[]>(employees => employees.Select(x=>x.Age), Is.All.GreaterThan(40));
    }
    
    [Test]
    public void GetWithQueryReturnsBadRequest()
    {
        Application
            .Http.Get
            .Url("/api/employee/older-than")
            .QueryParam("olderThan", -34)
            .Send()
            .Response
                .AssertStatusCode(HttpStatusCode.BadRequest);
    }

    [Test]
    public void GetReturnsNotFound()
    {
        Application
            .Http.Get
            .Url("/api/notfound")
            .Send()
            .Response
                .AssertStatusCode(HttpStatusCode.NotFound);
    }
}