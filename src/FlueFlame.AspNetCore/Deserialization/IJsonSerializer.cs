namespace FlueFlame.AspNetCore.Deserialization
{
    public interface ISerializer
    {
        T DeserializeObject<T>(string response);
        string SerializeObject(object value);
    }

    public interface IJsonSerializer : ISerializer
    {
    }
    
    public interface IXmlSerializer : ISerializer
    {
        
    }
}
