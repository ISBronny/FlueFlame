using FlueFlame.AspNetCore.Common;
using FlueFlame.AspNetCore.Deserialization;

namespace FlueFlame.AspNetCore.Modules.Response
{
    public class XmlResponseModule : AspNetModuleBase
    {
        private readonly IXmlSerializer _xmlSerializer;

        public XmlResponseModule(FlueFlameHost application) : base(application)
        {
            _xmlSerializer = Application.ServiceFactory.Get<IXmlSerializer>();
        }
    }
}