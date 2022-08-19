using System;
using FlueFlame.AspNetCore.Deserialization;
using FlueFlame.AspNetCore.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;

namespace FlueFlame.AspNetCore.Modules.Response.Content.Formatted;

public abstract class FormattedContentResponseModule : ContentResponseModule
{
    protected ISerializer Serializer { get; init; }
    protected FormattedContentResponseModule(FlueFlameHost application, string content) : base(application, content)
    {
       
    }
}

public abstract class FormattedContentResponseModule<T> : FormattedContentResponseModule where T : FormattedContentResponseModule<T>
{
    
    protected FormattedContentResponseModule(FlueFlameHost application, string content) : base(application, content)
    {
       
    }
    
    public T CopyResponseTo<TObject>(out TObject response)
    {
        response = Serializer.DeserializeObject<TObject>(Content);
        return (T) this;
    }
    
    public T AssertObject<TObject>(T expected)
    {
        var deserializedObject = Serializer.DeserializeObject<TObject>(Content);
        deserializedObject.Should().BeEquivalentTo(expected);
        return (T) this;
    }

    public T AssertProperty<TObject>(string propertyName, object expected)
    {
        throw new NotImplementedException();
        return (T) this;
    }
    
    public T AssertThat<TObject>(Action<TObject> constraint) 
    {
        var deserializedObject = Serializer.DeserializeObject<TObject>(Content);
        constraint(deserializedObject);
        return (T) this;
    }
}