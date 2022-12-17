using FlueFlame.Core.Serialization;
using Newtonsoft.Json;

namespace FlueFlame.Serialization.Newtonsoft
{
    internal class NewtonsoftJsonSerializer : IJsonSerializer
    {
        private readonly JsonSerializerSettings _settings;
        public NewtonsoftJsonSerializer(JsonSerializerSettings settings = null)
        {
            _settings = settings;
        }
        public T DeserializeObject<T>(string response)
        {
            return JsonConvert.DeserializeObject<T>(response, _settings);
        }

        public string SerializeObject(object value)
        {
            return JsonConvert.SerializeObject(value, _settings);
        }
    }
}
