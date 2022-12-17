using FlueFlame.Core.Serialization;
using FluentAssertions;

namespace FlueFlame.Core.Response.Content.Formatted;

public abstract class FormattedContentResponseModule<THost> : ContentResponseModule<THost> where THost : IFlueFlameHost
{
    protected ISerializer Serializer { get; init; }
    internal FormattedContentResponseModule(THost host, string content) : base(host, content)
    {
       
    }
}

public abstract class FormattedContentResponseModule<THost, TType> : FormattedContentResponseModule<THost> where TType : FormattedContentResponseModule<THost, TType> where THost : IFlueFlameHost
{
    
    internal FormattedContentResponseModule(THost host, string content) : base(host, content)
    {
       
    }
    
    /// <summary>
    /// Deserializes an object to TObject type and saves it to the specified variable.
    /// </summary>
    /// <param name="response">Variable where the response will be saved.</param>
    /// <typeparam name="TObject">The type of the object to deserialize to.</typeparam>
    /// <returns></returns>
    public TType CopyResponseTo<TObject>(out TObject response)
    {
        response = Serializer.DeserializeObject<TObject>(Content);
        return (TType) this;
    }
    
    /// <summary>
    /// Deserializes an object to TObject type and compares it to the expected object.
    /// </summary>
    /// <param name="expected">The object to which the response must be equivalent.</param>
    /// <typeparam name="TObject">The type of the object to deserialize to.</typeparam>
    /// <returns></returns>
    public TType AssertObject<TObject>(TObject expected)
    {
        var deserializedObject = Serializer.DeserializeObject<TObject>(Content);
        deserializedObject.Should().BeEquivalentTo(expected);
        return (TType) this;
    }

    /// <summary>
    /// Deserializes an object to TObject type and invokes specified action.
    /// The method is designed to test an assertion, but technically it can be used for any purpose.
    /// However, we recommend using it only for assertions.
    /// For other purposes, use <see cref="CopyResponseTo{TObject}"/>.
    /// </summary>
    /// <param name="constraint">Action that works with the body.</param>
    /// <typeparam name="TObject">The type of the object to deserialize to.</typeparam>
    /// <returns></returns>
    public TType AssertThat<TObject>(Action<TObject> constraint) 
    {
        var deserializedObject = Serializer.DeserializeObject<TObject>(Content);
        constraint(deserializedObject);
        return (TType) this;
    }
}