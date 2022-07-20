using FlueFlame.AspNetCore;
using FlueFlame.AspNetCore.Modules.Response;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace FlueFlame.Extensions.Assertions.NUnit;

public static class JsonResponseModuleExtensions
{
    public static JsonResponseModule AssertObject<T>(this JsonResponseModule module, T expected)
    {
        return new JsonResponseModuleWithAssertions(module.Application).AssertObject(expected);
    }
    public static JsonResponseModule AssertThat<T>(this JsonResponseModule module, Func<T, object> func, IResolveConstraint constraint) where T : class
    {
        return new JsonResponseModuleWithAssertions(module.Application).AssertThat(func, constraint);
    }
    public static JsonResponseModule AssertThat<T>(this JsonResponseModule module, IResolveConstraint constraint) where T : class
    {
        return new JsonResponseModuleWithAssertions(module.Application).AssertThat<T>(constraint);
    }
}

internal class JsonResponseModuleWithAssertions : JsonResponseModule
{
    public JsonResponseModuleWithAssertions(FlueFlameHost application) : base(application)
    {
        
    }
    
    public JsonResponseModule AssertObject<T>(T expected)
    {
        var text = BodyHelper.ReadAsText();
        var jResponse = JsonSerializer.DeserializeObject<T>(text);
        Assert.That(expected, Is.EqualTo(jResponse));
        return this;
    }
    public JsonResponseModule AssertThat<T>(Func<T, object> func, IResolveConstraint constraint) where T : class
    {
        var text = BodyHelper.ReadAsText();
        var jResponse = JsonSerializer.DeserializeObject<T>(text);
        Assert.That(func(jResponse), constraint);
        return this;
    }
    public JsonResponseModule AssertThat<T>(IResolveConstraint constraint) where T : class
    {
        var text = BodyHelper.ReadAsText();
        var jResponse = JsonSerializer.DeserializeObject<T>(text);
        Assert.That(jResponse, constraint);
        return this;
    }
}