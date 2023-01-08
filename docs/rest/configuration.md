# FlueFlameHttpHost Configuration 

`FlueFlameHttpHostBuilder` is used to create `IFlueFlameHttpHost`:

```csharp
var host = new FlueFlameHttpHostBuilder().Build();
```

## HttpClient Configuration

With the `ConfigureHttpClient` method, you have full access to `HttpClient` and can configure it however you like.

```csharp
var host = new FlueFlameHttpHostBuilder()
	.ConfigureHttpClient(client =>
	{
		//Base adress
		client.BaseAddress = new Uri("http://localhost:1234/");
		
		//JWT Authorization token
		client.DefaultRequestHeaders.Authorization =
			new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...");
		
		//Timeout
		client.Timeout = TimeSpan.FromSeconds(2);
	})
	.Build();
```

## Custom HttpClient

If you have your own `HttpClient` you can force `FlueFlame` to use it:

```csharp
var client = new HttpClient()
{
};

var host = new FlueFlameHttpHostBuilder()
	.UseCustomHttpClient(client)
	.Build();
```

## Serializers

By default `FlueFlame` uses `System.Text.Json.JsonSerializer` and `System.Xml.Serialization.XmlSerializer`.

It may be more convenient for you to use a different serializer.

For example, to use Newtonsoft.Json you can install the [FlueFlame.Serialization.Newtonsoft](https://www.nuget.org/packages/FlueFlame.Serialization.Newtonsoft) package:

```csharp
var host = new FlueFlameHttpHostBuilder()
	.UseNewtonsoftJsonSerializer()
	.Build();
```

To use all other serializers, you will have to implement the `IJsonSerializer` or `IXmlSerializer` interface.

Consider an example of the Newtonsoft.Json serializer, how you can use your own serializer. Сreate the `NewtonsoftJsonSerializer` class:

```csharp
public class NewtonsoftJsonSerializer : IJsonSerializer
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
```

And pass its instance to the `UseCustomJsonSerializer` method:

```csharp
var serializer = new NewtonsoftJsonSerializer(new JsonSerializerSettings()
{
	Formatting = Formatting.Indented
});

var host = new FlueFlameHttpHostBuilder()
	.UseCustomJsonSerializer(serializer)
	.Build();
```
