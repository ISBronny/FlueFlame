using System;
using FlueFlame.AspNetCore.Common;
using FlueFlame.AspNetCore.Deserialization;
using FlueFlame.AspNetCore.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;

namespace FlueFlame.AspNetCore.Modules.Response;

public abstract class FormattedResponseModule : AspNetModuleBase
{
    protected HttpResponse HttpResponse { get; }
    protected HttpResponseBodyHelper BodyHelper { get; }
    protected ISerializer Serializer = null;
    protected string ResponseText { get; }
    protected FormattedResponseModule(FlueFlameHost application) : base(application)
    {
        HttpResponse = Application.HttpContext.Response;
        BodyHelper = new HttpResponseBodyHelper(HttpResponse);
        ResponseText = BodyHelper.ReadAsText();
    }

    protected FormattedResponseModule(FormattedResponseModule module) : this(module.Application)
    {
        
    }
}

public abstract class FormattedResponseModule<T> : FormattedResponseModule where T : FormattedResponseModule<T>
{
    
    protected FormattedResponseModule(FlueFlameHost application) : base(application)
    {
        
    }
    
    public T CopyResponseTo<TObject>(out TObject response)
    {
        response = Serializer.DeserializeObject<TObject>(ResponseText);
        return (T) this;
    }
    
    public T AssertObject<TObject>(T expected)
    {
        var deserializedObject = Serializer.DeserializeObject<TObject>(ResponseText);
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
        var deserializedObject = Serializer.DeserializeObject<TObject>(ResponseText);
        constraint(deserializedObject);
        return (T) this;
    }
}