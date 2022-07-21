using FlueFlame.AspNetCore.Common;
using FlueFlame.AspNetCore.Deserialization;

namespace FlueFlame.AspNetCore.Modules.Response
{
    public class XmlResponseModule : FormattedResponseModule
    {
        protected readonly IXmlSerializer XmlSerializer;

        public XmlResponseModule(FlueFlameHost application) : base(application)
        {
            XmlSerializer = Application.ServiceFactory.Get<IXmlSerializer>();
        }
        
        public XmlResponseModule CopyResponseTo<T>(out T response)
        {
            var str = BodyHelper.ReadAsText();
            response = XmlSerializer.DeserializeObject<T>(str);
            return this;
        }
    }
}