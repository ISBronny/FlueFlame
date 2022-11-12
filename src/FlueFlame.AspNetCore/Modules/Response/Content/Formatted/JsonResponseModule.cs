using FlueFlame.AspNetCore.Deserialization;

namespace FlueFlame.AspNetCore.Modules.Response.Content.Formatted
{
    public sealed class JsonContentResponseModule : FormattedContentResponseModule<JsonContentResponseModule>
    {
        internal JsonContentResponseModule(IFlueFlameHost application, string content) : base(application, content)
        {
            Serializer = Application.ServiceFactory.Get<IJsonSerializer>();
        }
    }
}