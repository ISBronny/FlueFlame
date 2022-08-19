using FlueFlame.AspNetCore.Deserialization;

namespace FlueFlame.AspNetCore.Modules.Response.Content.Formatted
{
    public class XmlContentResponseModule : FormattedContentResponseModule<XmlContentResponseModule>
    {
        public XmlContentResponseModule(FlueFlameHost application, string content) : base(application, content)
        {
            Serializer = Application.ServiceFactory.Get<IXmlSerializer>();
        }
    }
}