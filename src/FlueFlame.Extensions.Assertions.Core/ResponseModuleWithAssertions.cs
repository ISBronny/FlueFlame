using System.Net;
using FlueFlame.AspNetCore;
using FlueFlame.AspNetCore.Modules.Response;

namespace FlueFlame.Extensions.Assertions.Core;

public abstract class ResponseModuleWithAssertions : ResponseModule
{
    public ResponseModuleWithAssertions(FlueFlameHost application) : base(application)
    {
        
    }

    #region Status Code

    public abstract ResponseModule AssertStatusCode(HttpStatusCode statusCode);

    public abstract ResponseModule AssertStatusCode(int statusCode);

    #endregion
    
    #region Headers

    public abstract ResponseModule AssertContainsHeaders(params string[] headers);

    public abstract ResponseModule AssertDoesNotContainsHeaders(params string[] headers);

    public abstract ResponseModule AssertHeader(string key, string value);

    public abstract ResponseModule AssertHeaderPattern(string key, string pattern);

    #endregion

    public abstract ResponseModule AssertBodyLength(int length);
}