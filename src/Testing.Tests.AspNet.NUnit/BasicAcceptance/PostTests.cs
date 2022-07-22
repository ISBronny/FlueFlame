using System.Net;
using FlueFlame.Extensions.Assertions.NUnit;
using Testing.TestData.AspNetCore.Models;

namespace Testing.Tests.AspNet.NUnit.BasicAcceptance;

public class PostTests : TestBase
{
    private static Employee ValidEmployee => new()
    {
        Age = 23,
        Position = "Php Junior Developer",
        FullName = "Alex Grow"
    };
    
    private static Employee InvalidEmployee => new()
    {
        Age = -45,
        Position = "Php Junior Developer",
        FullName = "Alex Grow"
    };
        
    [Test]
    public void PostReturnsOk()
    {
        Application
            .Http.Post
            .Url("/api/employee")
            .Json(ValidEmployee)
            .Send()
            .Response
                .AssertStatusCode(HttpStatusCode.OK);
    }

    [Test]
    public void GetReturnsBadRequest()
    {
        Application
            .Http.Post
            .Url("/api/employee")
            .Json(InvalidEmployee)
            .Send()
            .Response
                .AssertStatusCode(HttpStatusCode.BadRequest);
    }
}