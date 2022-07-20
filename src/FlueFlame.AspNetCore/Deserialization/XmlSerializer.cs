using System.IO;

namespace FlueFlame.AspNetCore.Deserialization;

public class XmlSerializer : IXmlSerializer
{
    public T DeserializeObject<T>(string response)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(response);
        return (T) new System.Xml.Serialization.XmlSerializer(typeof(T)).Deserialize(stream);
    }

    public string SerializeObject(object value)
    {
        var writer = new StringWriter();
        var serializer = new System.Xml.Serialization.XmlSerializer(value.GetType());
        serializer.Serialize(writer, value);
        return writer.ToString();
    }
}