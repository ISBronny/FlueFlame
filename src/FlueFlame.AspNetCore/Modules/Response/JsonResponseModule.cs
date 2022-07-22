using FlueFlame.AspNetCore.Deserialization;

namespace FlueFlame.AspNetCore.Modules.Response
{
    public class JsonResponseModule : FormattedResponseModule<JsonResponseModule>
    {
        public JsonResponseModule(FlueFlameHost application) : base(application)
        {
            Serializer = Application.ServiceFactory.Get<IJsonSerializer>();
        }
    }
}