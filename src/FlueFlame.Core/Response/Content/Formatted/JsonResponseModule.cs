using FlueFlame.Core.Serialization;

namespace FlueFlame.Core.Response.Content.Formatted
{
    public sealed class JsonContentResponseModule<THost> : FormattedContentResponseModule<THost, JsonContentResponseModule<THost>> where THost : IFlueFlameHost
    {
        public JsonContentResponseModule(THost host, IJsonSerializer jsonSerializer, string content) : base(host, content)
        {
            Serializer = jsonSerializer;
        }
    }
}