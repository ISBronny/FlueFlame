using FlueFlame.AspNetCore.Deserialization;

namespace FlueFlame.AspNetCore.Modules.Response.Content.Formatted
{
    public class JsonContentResponseModule : FormattedContentResponseModule<JsonContentResponseModule>
    {
        public JsonContentResponseModule(FlueFlameHost application, string content) : base(application, content)
        {
            Serializer = Application.ServiceFactory.Get<IJsonSerializer>();
        }
    }
}