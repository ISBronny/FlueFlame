namespace FlueFlame.AspNetCore.Deserialization
{
    internal class TextJsonJsonSerializer : IJsonSerializer
    {
        public T DeserializeObject<T>(string response)
        {
            return System.Text.Json.JsonSerializer.Deserialize<T>(response);
        }

        public string SerializeObject(object value)
        {
            return System.Text.Json.JsonSerializer.Serialize(value);
        }
    }
}
