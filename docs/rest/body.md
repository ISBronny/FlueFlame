# Request with Body

FlueFlame allows you to set the request body. It can be either plain text or a JSON or XML object.

## Test with request body

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
     Application
        .Http.Post
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
     Application
        .Http.Post
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