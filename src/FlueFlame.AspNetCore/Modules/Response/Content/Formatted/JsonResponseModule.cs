using FlueFlame.AspNetCore.Deserialization;

namespace FlueFlame.AspNetCore.Modules.Response.Content.Formatted
{
    public sealed class JsonContentResponseModule : FormattedContentResponseModule<JsonContentResponseModule>
    {
        internal JsonContentResponseModule(IFlueFlameHost application, IJsonSerializer jsonSerializer, string content) : base(application, content)
        {
            Serializer = jsonSerializer;
        }
    }
}