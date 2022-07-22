using System.Net.Mime;
using FlueFlame.AspNetCore.Common;
using FlueFlame.AspNetCore.Deserialization;
using FlueFlame.AspNetCore.Services;
using Microsoft.AspNetCore.Http;

namespace FlueFlame.AspNetCore.Modules.Response;

public abstract class FormattedResponseModule : AspNetModuleBase
{
    protected HttpResponse HttpResponse { get; }
    protected HttpResponseBodyHelper BodyHelper { get; }
    protected ISerializer Serializer = null;
    protected FormattedResponseModule(FlueFlameHost application) : base(application)
    {
        HttpResponse = Application.HttpContext.Response;
        BodyHelper = new HttpResponseBodyHelper(HttpResponse);
    }

    protected FormattedResponseModule(FormattedResponseModule module) : this(module.Application)
    {
        Serializer = module.Serializer;
    }
}

public abstract class FormattedResponseModule<T> : FormattedResponseModule where T : FormattedResponseModule<T>
{
    protected FormattedResponseModule(FlueFlameHost application) : base(application)
    {
    }
    
    public T CopyResponseTo<TObject>(out TObject response)
    {
        var str = BodyHelper.ReadAsText();
        response = Serializer.DeserializeObject<TObject>(str);
        return (T)this;
    }
}