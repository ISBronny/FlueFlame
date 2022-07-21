using System.Net;
using FlueFlame.AspNetCore;
using FlueFlame.Extensions.Assertions.NUnit;

namespace Testsing.Tests.AspNet.NUnit.Authentication.JWT;

public class JwtTests : TestBase
{

    [Test]
    public void AuthWithJwtReturnsOk()
    {
        Application
            .CreateJwt("admin@gmail.com", "12345", out var token)
            .Http.Get
            .Url("/api/admin/test")
            .WithBearerToken(token)
            .Send()
            .Response
                .AssertStatusCode(HttpStatusCode.OK);
    }
    
    [Test]
    public void AuthWithJwtReturnsForbidden()
    {
        Application
            .CreateJwt("qwerty@gmail.com", "55555", out var token)
            .Http.Get
            .Url("/api/admin/test")
            .WithBearerToken(token)
            .Send()
            .Response
                .AssertStatusCode(HttpStatusCode.Forbidden);
    }
}

public static class FlueFlameExtension
{
    public static FlueFlameHost CreateJwt(this FlueFlameHost application, string username, string password, out string token)
    {
        return application
            .Http.Post
            .Url("/token")
            .QueryParam("username", username)
            .QueryParam("password", password)
            .Send()
            .Response
                .AssertStatusCode(HttpStatusCode.OK)
                .AsText.CopyResponseTo(out token).Application;
    }
}