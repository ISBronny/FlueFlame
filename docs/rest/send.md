# Send requests to REST API

`HttpModule` is used to test the REST API. It allows you to serialize objects, request at a specified URL, set the necessary headers, and provide access to the response.

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

`IFlueFlameHttpHost` provides access to `HttpModule` via `Get`, `Post`, `Delete`, etc. properties.
`HttpModule` contains methods for setting up an HTTP request and a `Send()` method for sending a request.

```csharp
[Test]
public void GetReturnsOk()
{
    HttpHost.Get
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
    HttpHost.Get
        .Url("/api/employee/older-than")
        .AddQuery("olderThan", 45)
        .Send()
}
```

## Request with body

We have an `Create` action in our `EmployeeController` that adds a new employee to the database:

```csharp
[Route("")]
[HttpPost]
public async Task<IActionResult> Create([FromBody] Employee employee)
{
    if(!ModelState.IsValid)
        return BadRequest(ModelState);
    return Ok(await _employeeRepository.Add(employee));
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
     HttpHost.Post
        .Url("/api/employee")
        .Json(ValidEmployee)
        .Send()
}
```

The serializer specified in `TestApplicationBuilder` will be used for serialization. `System.Text` used by default.

::: tip

We don't recommend instantiating an object inside a test, as shown below, as this makes the test less readable.


```csharp   
[Test]
public void PostReturnsOk()
{
     HttpHost.Post
        .Url("/api/employee")
        .Json(new Employee() 
        {       
            Age = 23,
            Position = "Php Junior Developer",
            FullName = "Alex Grow"
        })
        .Send()
}
```

:::

