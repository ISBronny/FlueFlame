using FlueFlame.AspNetCore.Modules.Response;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace FlueFlame.Extensions.Assertions.NUnit;

public static class FormattedResponseModuleExtensions
{
    public static FormattedResponseModule AssertObject<T>(this FormattedResponseModule module, T expected)
    {
        return new FormattedResponseModuleWithAssertions(module).AssertObject(expected);
    }
    public static FormattedResponseModule AssertThat<T>(this FormattedResponseModule module, Func<T, object> func, IResolveConstraint constraint) where T : class 
    {
        return new FormattedResponseModuleWithAssertions(module).AssertThat(func, constraint);
    }
    public static FormattedResponseModule AssertThat<T>(this FormattedResponseModule module, IResolveConstraint constraint) where T : class
    {
        return new FormattedResponseModuleWithAssertions(module).AssertThat<T>(constraint);
    }
}

internal class FormattedResponseModuleWithAssertions : FormattedResponseModule
{
    public FormattedResponseModuleWithAssertions(FormattedResponseModule formattedResponseModule) : base(formattedResponseModule)
    {
    }
    
    public FormattedResponseModule AssertObject<T>(T expected)
    {
        var text = BodyHelper.ReadAsText();
        var jResponse = Serializer.DeserializeObject<T>(text);
        Assert.That(expected, Is.EqualTo(jResponse));
        return this;
    }
    public FormattedResponseModule AssertThat<T>(Func<T, object> func, IResolveConstraint constraint) where T : class
    {
        var text = BodyHelper.ReadAsText();
        var jResponse = Serializer.DeserializeObject<T>(text);
        Assert.That(func(jResponse), constraint);
        return this;
    }
    public FormattedResponseModule AssertThat<T>(IResolveConstraint constraint) where T : class
    {
        var text = BodyHelper.ReadAsText();
        var jResponse = Serializer.DeserializeObject<T>(text);
        Assert.That(jResponse, constraint);
        return this;
    }
}