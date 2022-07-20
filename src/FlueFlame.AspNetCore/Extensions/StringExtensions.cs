using Newtonsoft.Json;

namespace FlueFlame.AspNetCore.Extensions;

public static class StringExtensions
{
    public static string ToIndentedJson(this string json)
    {
        dynamic parsedJson = JsonConvert.DeserializeObject(json);
        return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
    }
    
    public static dynamic ToJson(this string json)
    {
        return JsonConvert.DeserializeObject(json);
    }
}