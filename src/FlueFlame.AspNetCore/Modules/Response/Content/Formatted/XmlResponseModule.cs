using FlueFlame.AspNetCore.Deserialization;

namespace FlueFlame.AspNetCore.Modules.Response.Content.Formatted
{
    public sealed class XmlContentResponseModule : FormattedContentResponseModule<XmlContentResponseModule>
    {
        internal XmlContentResponseModule(IFlueFlameHost application, IXmlSerializer xmlSerializer, string content) : base(application, content)
        {
            Serializer = xmlSerializer;
        }
    }
}