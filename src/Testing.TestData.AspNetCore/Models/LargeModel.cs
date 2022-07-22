using System.Text.Json.Serialization;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Testing.TestData.AspNetCore.Models;

[XmlRoot(ElementName = nameof(LargeModel), Namespace = "Testing.TestData.AspNetCore.Models")]
public class LargeModel
{
    [JsonProperty("id")]
    [JsonPropertyName("id")]
    //[XmlElement("Id")]
    public int Id { get; set; }
    
    [JsonProperty("field")]
    [JsonPropertyName("field")]
    //[XmlElement("Field")]
    public string String { get; set; }
    
    [JsonProperty("time")]
    [JsonPropertyName("time")]
    //[XmlElement("Time")]
    public DateTime Time { get; set; }
    
    [JsonProperty("children")]
    [JsonPropertyName("children")]
    //[XmlArray("Children")]
    //[XmlArrayItem("ChildLargeModel", typeof(ChildLargeModel))]
    public List<ChildLargeModel> Children { get; set; }
}


public class ChildLargeModel
{
    [JsonProperty("id")]
    [JsonPropertyName("id")]
    //[XmlElement("Id")]
    public int Id { get; set; }
    
    [JsonProperty("values")]
    [JsonPropertyName("values")]
    //[XmlArray("Values")]
    //[XmlArrayItem("ValueObject", typeof(ValueObject))]
    public ValueObject[] Values { get; set; }
}

public class ValueObject
{
    [JsonProperty("value")]
    [JsonPropertyName("value")]
    //[XmlElement("Value")]
    public string Value { get; set; }
}