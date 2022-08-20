using FlueFlame.AspNetCore.Deserialization;

namespace FlueFlame.AspNetCore.Modules.Response.Content.Formatted
{
    public sealed class XmlContentResponseModule : FormattedContentResponseModule<XmlContentResponseModule>
    {
        internal XmlContentResponseModule(FlueFlameHost application, string content) : base(application, content)
        {
            Serializer = Application.ServiceFactory.Get<IXmlSerializer>();
        }
    }
}