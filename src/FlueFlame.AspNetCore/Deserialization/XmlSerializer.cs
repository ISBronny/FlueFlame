using System.IO;
using System.Xml;

namespace FlueFlame.AspNetCore.Deserialization;

public class XmlSerializer : IXmlSerializer
{
    public T DeserializeObject<T>(string response)
    {
        var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
        using TextReader reader = new StringReader(response);
        return (T)serializer.Deserialize(reader);
    }

    public string SerializeObject(object value)
    {
        var writer = new StringWriter();
        var serializer = new System.Xml.Serialization.XmlSerializer(value.GetType());
        serializer.Serialize(writer, value);
        return writer.ToString();
    }
}