using FlueFlame.AspNetCore.Common;
using Microsoft.AspNetCore.Http;


namespace FlueFlame.AspNetCore.Modules.Response
{
    public class ResponseModule : AspNetModuleBase
    {
        protected HttpResponse HttpResponse { get; }
        public ResponseModule(FlueFlameHost application) : base(application)
        {
            HttpResponse = Application.HttpContext.Response;
        }

        public JsonResponseModule AsJson => new JsonResponseModule(Application);
        public XmlResponseModule AsXml => new XmlResponseModule(Application);
    }
}