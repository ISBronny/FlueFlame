using System.Text.Json;

namespace FlueFlame.Core.Serialization
{
    public class TextJsonSerializer : IJsonSerializer
    {
        private readonly JsonSerializerOptions _options;

        public TextJsonSerializer(JsonSerializerOptions options = null)
        {
            _options = options;
        }

        public T DeserializeObject<T>(string response)
        {
            return JsonSerializer.Deserialize<T>(response, _options);
        }

        public string SerializeObject(object value)
        {
            return JsonSerializer.Serialize(value, _options);
        }
    }
}
