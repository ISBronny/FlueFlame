using System.Net;
using FlueFlame.AspNetCore;

namespace Testing.Tests.AspNet.NUnit.Authentication.JWT;

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
            .HttpResponse
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
            .HttpResponse
                .AssertStatusCode(HttpStatusCode.Forbidden);
    }
}

public static class FlueFlameExtension
{
    public static IFlueFlameHost CreateJwt(this IFlueFlameHost application, string username, string password, out string token)
    {
        return application
            .Http.Post
            .Url("/token")
            .AddQuery("username", username)
            .AddQuery("password", password)
            .Send()
            .HttpResponse
                .AssertStatusCode(HttpStatusCode.OK)
                .AsText.CopyResponseTo(out token).Application;
    }
}