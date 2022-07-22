using FlueFlame.AspNetCore.Common;
using FlueFlame.AspNetCore.Deserialization;

namespace FlueFlame.AspNetCore.Modules.Response
{
    public class XmlResponseModule : FormattedResponseModule<XmlResponseModule>
    {
        public XmlResponseModule(FlueFlameHost application) : base(application)
        {
            Serializer = Application.ServiceFactory.Get<IXmlSerializer>();
        }
    }
}