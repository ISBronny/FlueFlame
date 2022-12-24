# Response

Obviously you will want to access the response in your tests. FlueFlame allows you to check response parameters such as headers or status code and deserialize the response body for further work with it.

## Assert HTTP Status Code and Headers

The `Response` property is used to access the response. `Response` contains methods for checking the status code and headers.

```csharp
[Test]
public void GetReturnsOk()
{
    HttpHost.Get
        .Url("/api/employee/all")
        .Send()
        .Response
            .AssertStatusCode(HttpStatusCode.OK)
            .AssertHeader("MyCustomHeaderKey", "MyCustomHeaderValue");
}
```

## Assert Response Body

To check the body, use the `AsJson`, `AsXml` or `AsText` properties. They indicate how to serialize the response in the next methods.

```csharp
[Test]
public void GetWithQueryReturnsOk()
{
    HttpHost.Get
        .Url("/api/employee/older-than")
        .AddQuery("olderThan", 45)
        .Send()
        .Response
            .AssertStatusCode(HttpStatusCode.OK)
            .AsJson
                .AssertThat<Employee[]>(employees => employees.Should().NotContain(x => x.Age < 45));
}
```

The `AssertThat` method specifies the type into which the response is to be serialized and the method to be called on the serialized object. FlueFlame comes with the [FluentAssertions](https://fluentassertions.com/) library as it fits the concept nicely, but you can use any library for entity validation.

## Copy Response

You can also copy the deserialized object for further work with it using the `CopyResponseTo()` method.

```csharp
[Test]
public void GetWithQueryReturnsOk()
{
    HttpHost.Get
        .Url("/api/employee/older-than")
        .AddQuery("olderThan", 45)
        .Send()
        .Response
            .AssertStatusCode(HttpStatusCode.OK)
            .AsJson
                .CopyResponseTo(out Employee[] employees);

    var age = employees.First().Age;
    //... your code
}
```