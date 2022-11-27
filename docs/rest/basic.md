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



