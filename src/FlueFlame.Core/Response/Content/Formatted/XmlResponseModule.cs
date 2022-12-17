using FlueFlame.Core.Serialization;

namespace FlueFlame.Core.Response.Content.Formatted
{
    public sealed class XmlContentResponseModule<THost> : FormattedContentResponseModule<THost, XmlContentResponseModule<THost>> where THost : IFlueFlameHost
    {
        public XmlContentResponseModule(THost host, IXmlSerializer xmlSerializer, string content) : base(host, content)
        {
            Serializer = xmlSerializer;
        }
    }
}