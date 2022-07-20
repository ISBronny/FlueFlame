using FlueFlame.AspNetCore.Common;
using FlueFlame.AspNetCore.Services;
using Microsoft.AspNetCore.Http;

namespace FlueFlame.AspNetCore.Modules.Response;

public abstract class FormattedResponseModule : AspNetModuleBase
{
    protected HttpResponse HttpResponse { get; }
    protected HttpResponseBodyHelper BodyHelper { get; }
    protected FormattedResponseModule(FlueFlameHost application) : base(application)
    {
        HttpResponse = Application.HttpContext.Response;
        BodyHelper = new HttpResponseBodyHelper(HttpResponse);
    }
}