using System.Net;
using FlueFlame.AspNetCore;
using FlueFlame.AspNetCore.Modules.Response;
using NUnit.Framework;

namespace FlueFlame.Extensions.Assertions.NUnit;

public static class ResponseModuleExtension
{
    public static ResponseModule AssertStatusCode(this ResponseModule module, HttpStatusCode statusCode)
    {
        return new ResponseModuleWithAssertions(module.Application).AssertStatusCode(statusCode);
    }
}

internal class ResponseModuleWithAssertions : ResponseModule
{
    public ResponseModuleWithAssertions(FlueFlameHost application) : base(application)
    {
    }

    public ResponseModule AssertStatusCode(HttpStatusCode statusCode)
    {
        AssertStatusCode((int)statusCode);
        return this;
    }
    
    public ResponseModule AssertStatusCode(int statusCode)
    {
        Assert.AreEqual(statusCode, HttpResponse.StatusCode);
        return this;
    }
}