using Newtonsoft.Json;

namespace FlueFlame.AspNetCore.Deserialization
{
    internal class NewtonsoftJsonSerializer : IJsonSerializer
    {
        private readonly JsonSerializerSettings _settings;
        public NewtonsoftJsonSerializer()
        {
        }
        public NewtonsoftJsonSerializer(JsonSerializerSettings settings)
        {
            _settings = settings;
        }
        public T DeserializeObject<T>(string response)
        {
            return JsonConvert.DeserializeObject<T>(response, _settings);
        }

        public string SerializeObject(object value)
        {
            return JsonConvert.SerializeObject(value);
        }
    }
}
