using FlueFlame.AspNetCore.Deserialization;

namespace FlueFlame.AspNetCore.Modules.Response
{
    public class JsonResponseModule : FormattedResponseModule
    {
        protected readonly IJsonSerializer JsonSerializer;

        public JsonResponseModule(FlueFlameHost application) : base(application)
        {
            JsonSerializer = Application.ServiceFactory.Get<IJsonSerializer>();
        }

        public JsonResponseModule CopyResponseTo<T>(out T response)
        {
            var str = BodyHelper.ReadAsText();
            response = JsonSerializer.DeserializeObject<T>(str);
            return this;
        }

    }
}