using FlueFlame.AspNetCore.Common;
using FlueFlame.AspNetCore.Services;
using Microsoft.AspNetCore.Http;

namespace FlueFlame.AspNetCore.Modules.Response
{
    public class TextResponseModule : AspNetModuleBase
    {
        protected HttpResponse HttpResponse { get; }
        protected HttpResponseBodyHelper BodyHelper { get; }

        public TextResponseModule(FlueFlameHost application) : base(application)
        {
            HttpResponse = Application.HttpContext.Response;
            BodyHelper = new HttpResponseBodyHelper(HttpResponse);
        }
        
        public TextResponseModule CopyResponseTo(out string response)
        {
            response = BodyHelper.ReadAsText();
            return this;
        }
    }
}