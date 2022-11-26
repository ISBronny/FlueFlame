# Send requests to REST API

## Simple Request

Let's say we have a simple `EmployeeController` with a `GetAll()` action that returns a list of all employees.

```csharp
[ApiController]
[Route("api/employee")]
public class EmployeeController : ControllerBase
{
    private IEmployeeRepository _employeeRepository;

    public EmployeeController(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }
    
    [Route("all")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _employeeRepository.GetAll());
    }

}
```

To send requests to the REST API, `HttpFacade` and `HttpModule` are used.
`HttpFacade` provides access to `HttpModule` via `Get`, `Post`, `Delete`, etc. properties.
`HttpModule` contains methods for setting up an HTTP request and a `Send()` method for sending a request.

```csharp
[Test]
public void GetReturnsOk()
{
    Application
        .Http.Get
        .Url("/api/employee/all")
        .Send()
}
```

## Request with Query

Let's add a `GetByOlderThan` action to our `EmployeeController` that returns employees older than a certain age:

```csharp
[Route("older-than")]
[HttpGet]
public async Task<IActionResult> GetByOlderThan([FromQuery(Name = "olderThan"), Range(18, 99)] int olderThan)
{
    if(ModelState.IsValid)
        return Ok(await _employeeRepository.OlderThan(olderThan));
    return BadRequest(ModelState);
}
```

The `AddQuery()` method is used to add parameters.

```csharp
[Test]
public void GetWithQueryReturnsOk()
{
    Application
        .Http.Get
        .Url("/api/employee/older-than")
        .AddQuery("olderThan", 45)
        .Send()
}
```

## Request with Body

Let's add a `Create` action to our `EmployeeController` that adds a new employee to the database:

```csharp
[Route("")]
[HttpPost]
public async Task<IActionResult> Create([FromBody] Employee employee)
{
    if(ModelState.IsValid)
        return Ok(await _employeeRepository.Add(employee));
    return BadRequest(ModelState);
}
```

To set the request body, you can call the `Json()`, `Xml()` or `Text()` methods of the `HttpModule`. They will also set the appropriate `ContentType` and `Accept` headers.

```csharp
private static Employee ValidEmployee => new()
{
    Age = 23,
    Position = "Php Junior Developer",
    FullName = "Alex Grow"
};
        
[Test]
public void PostReturnsOk()
{
     Application
        .Http.Post
        .Url("/api/employee")
        .Json(ValidEmployee)
        .Send()
}
```

`System.Text` will be used by default for serialization.

## Read response

The `HttpResponse` property is used to access the response. `HttpResponse` contains asserts for checking the status of the response code and headers.

```csharp
[Test]
public void GetReturnsOk()
{
    Application
        .Http.Get
        .Url("/api/employee/all")
        .Send()
        .HttpResponse
            .AssertStatusCode(HttpStatusCode.OK);
}
```

To check the body, use the `AsJson`, `AsXml` or `AsText` properties.

```csharp
[Test]
public void GetWithQueryReturnsOk()
{
    Application
        .Http.Get
        .Url("/api/employee/older-than")
        .AddQuery("olderThan", 45)
        .Send()
        .HttpResponse
            .AssertStatusCode(HttpStatusCode.OK)
            .AsJson
                .AssertThat<Employee[]>(employees => employees.Should().NotContain(x=>x.Age<45));
}
```

The `AssertThat` method specifies the type into which the response is to be serialized and the method to be called on the serialized object.

You can also copy the deserialized object for further work with it using the `CopyResponseTo()` method.

```csharp
[Test]
public void GetWithQueryReturnsOk()
{
    Application
        .Http.Get
        .Url("/api/employee/older-than")
        .AddQuery("olderThan", 45)
        .Send()
        .HttpResponse
            .AssertStatusCode(HttpStatusCode.OK)
            .AsJson
                .CopyResponseTo(out Employee[] employees);

    var age = employees.First().Age;
    //... your code
}
```