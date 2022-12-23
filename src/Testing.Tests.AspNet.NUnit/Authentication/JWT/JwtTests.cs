using System.Net;
using FlueFlame.Http.Host;

namespace Testing.Tests.AspNet.NUnit.Authentication.JWT;

public class JwtTests : TestBase
{

    [Test]
    public void AuthWithJwtReturnsOk()
    {
        Http
            .CreateJwt("admin@gmail.com", "12345", out var token)
            .Get
                .Url("/api/admin/test")
                .WithJwtToken(token)
                .Send()
                .Response
                    .AssertStatusCode(HttpStatusCode.OK);
    }
    
    [Test]
    public void AuthWithJwtReturnsForbidden()
    {
        Http
            .CreateJwt("qwerty@gmail.com", "55555", out var token)
            .Get
                .Url("/api/admin/test")
                .WithJwtToken(token)
                .Send()
                .Response
                    .AssertStatusCode(HttpStatusCode.Forbidden);
    }
}

public static class FlueFlameExtension
{
    public static IFlueFlameHttpHost CreateJwt(this IFlueFlameHttpHost http, string username, string password, out string token)
    {
        return http.Post
            .Url("/token")
            .AddQuery("username", username)
            .AddQuery("password", password)
            .Send()
            .Response
                .AssertStatusCode(HttpStatusCode.OK)
                .AsText.CopyResponseTo(out token).Host;
    }
}