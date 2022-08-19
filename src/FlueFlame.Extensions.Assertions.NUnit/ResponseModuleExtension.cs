using System.Net;
using FlueFlame.AspNetCore;
using FlueFlame.AspNetCore.Modules.Response;
using FlueFlame.Extensions.Assertions.Core;
using Microsoft.Extensions.Primitives;
using NUnit.Framework;

namespace FlueFlame.Extensions.Assertions.NUnit;

public static class ResponseModuleExtension
{

    #region Status Code

    public static ResponseModule AssertStatusCode(this ResponseModule module, HttpStatusCode statusCode)
    {
        return new ResponseModuleWithNUnit(module.Application).AssertStatusCode(statusCode);
    }

    public static ResponseModule AssertStatusCode(this ResponseModule module, int statusCode)
    {
        return new ResponseModuleWithNUnit(module.Application).AssertStatusCode(statusCode);
    }

    #endregion
    
    #region Headers

    public static ResponseModule AssertContainsHeaders(this ResponseModule module, params string[] headers)
    {
        return new ResponseModuleWithNUnit(module.Application).AssertContainsHeaders(headers);
    }

    public static ResponseModule AssertDoesNotContainsHeaders(this ResponseModule module, params string[] headers)
    {
        return new ResponseModuleWithNUnit(module.Application).AssertDoesNotContainsHeaders(headers);
    }

    public static ResponseModule AssertHeader(this ResponseModule module, string key, string value)
    {
        return new ResponseModuleWithNUnit(module.Application).AssertHeader(key, value);
    }

    public static ResponseModule AssertHeaderPattern(this ResponseModule module, string key, string pattern)
    {
        return new ResponseModuleWithNUnit(module.Application).AssertHeaderPattern(key, pattern);
    }

    #endregion

    public static ResponseModule AssertBodyLength(this ResponseModule module, int length)
    {
        return new ResponseModuleWithNUnit(module.Application).AssertBodyLength(length);
    }
}

internal class ResponseModuleWithNUnit : ResponseModuleWithAssertions
{
    public ResponseModuleWithNUnit(FlueFlameHost application) : base(application)
    {
        
    }

    #region Status Code
    
    public override ResponseModule AssertStatusCode(HttpStatusCode statusCode)
    {
        AssertStatusCode((int)statusCode);
        return this;
    }
    
    public override ResponseModule AssertStatusCode(int statusCode)
    {
        Assert.AreEqual(statusCode, HttpResponse.StatusCode);
        return this;
    }
    
    #endregion
    
    #region Headers

    public override ResponseModule AssertContainsHeaders(params string[] headers)
    {
        Assert.That(HttpResponse.Headers.Select(x=>x.Key), Is.EquivalentTo(headers));
        return this;
    }
    
    public override ResponseModule AssertDoesNotContainsHeaders(params string[] headers)
    {
        Assert.That(HttpResponse.Headers.Select(x=>x.Key), Is.All.Not.AnyOf(headers));
        return this;
    }

    public override ResponseModule AssertHeader(string key, string value)
    {
        Assert.That(HttpResponse.Headers, Does.Contain(new KeyValuePair<string, StringValues>(key, value)));
        return this;
    }
    
    public override ResponseModule AssertHeaderPattern(string key, string pattern)
    {
        Assert.That(HttpResponse.Headers[key], Does.Match(pattern));
        return this;
    }

    #endregion
    
    public override ResponseModule AssertBodyLength(int length)
    {
        Assert.AreEqual(HttpResponse.Body.Length, length);
        return this;
    }
}