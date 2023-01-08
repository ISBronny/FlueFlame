# Getting Started

In this section, we will create a simple REST API test. We will use the [Mocky](https://designer.mocky.io/) service as endpoint. For testing ASP.NET Core, read [ASP.NET Core Integration](/rest/asp-net).

## Mock backend

Using the [Mocky](https://designer.mocky.io/) service, create an endpoint that returns a simple object in JSON format:

```json
{
    "id" : 7384,
    "name" : "Elon Mask"
}
```
It can be received from the link https://run.mocky.io/v3/d1bc72d3-5325-4436-9261-421bff34c30f

## TestBase
Let's create a test project. We will use [xUnit](), but you can use any other framework.
In the test project, create a `TestBase` class:

```csharp
public abstract class TestBase
{

	public TestBase()
	{

	}
}
```


Now we need to create and configure `IFlueFlameHttpHost`. To do this, we use `FlueFlameHttpHostBuilder`:

```csharp
public abstract class TestBase
{
	protected IFlueFlameHttpHost HttpHost { get; }

	public TestBase()
	{
		HttpHost = new FlueFlameHttpHostBuilder()
			.ConfigureHttpClient(client =>
			{
				client.BaseAddress = new Uri("https://run.mocky.io/");
			})
			.Build();
	}
}
```

## Response Class

In order for FlueFlame to be able to deserialize the response, we need to create a model:

```csharp
public class HumanViewModel
{
	[JsonPropertyName("id")]
	public int Id { get; set; }

	[JsonPropertyName("name")]
	public string Name { get; set; }
}
```

## Test Endpoint

Create a class to test our endpoint:

```csharp

public class MockyTest : TestBase
{
	[Fact]
	public void Mocky_ReturnsHuman()
	{
		
	}
}

```

The following code will send an HTTP request to the desired URL:

```csharp

public class MockyTest : TestBase
{
	[Fact]
	public void Mocky_ReturnsHuman()
	{
		HttpHost.Get
			.Url("/v3/d1bc72d3-5325-4436-9261-421bff34c30f")
			.Send()
	}
}

```

Check that a response with a 200 (OK) code was returned:

```csharp
[Fact]
public void Mocky_ReturnsHuman()
{
	HttpHost.Get
		.Url("/v3/d1bc72d3-5325-4436-9261-421bff34c30f")
		.Send()
		.Response
			.AssertStatusCode(HttpStatusCode.OK)
}
```

::: tip 
Please note that each semantic block of the test is highlighted with an additional tabulation. We recommend using this codestyle as it improves the readability of the code.
:::

And finally check the body of the response:

```csharp
[Fact]
public void Mocky_ReturnsHuman()
{
	HttpHost.Get
		.Url("/v3/d1bc72d3-5325-4436-9261-421bff34c30f")
		.Send()
		.Response
			.AssertStatusCode(HttpStatusCode.OK)
			.AsJson
				.AssertThat<HumanViewModel>(r => r.Name.Should().Be("Elon Mask"))
				.AssertThat<HumanViewModel>(r => r.Id.Should().Be(7384));
}
```

::: tip 
For assertions, the [FluentAssertions](https://fluentassertions.com/) library was used. This is one of the `FlueFlame` package dependencies and we think FluentAssertions fits the concept perfectly. However, you can use any other means of testing.
:::

## What is next?

You have learned the basics of end-to-end testing with `FlueFlame`. Now you can learn how to [integrate](/rest/asp-net) FlueFlame with ASP.NET Core, use [authorization](/rest/auth), fully [manage](/rest/send) your HTTP requests, and use [custom](/rest/configuration#serializers) serialization.





