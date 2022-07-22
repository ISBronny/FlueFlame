using Microsoft.AspNetCore.Mvc;

namespace Testing.TestData.AspNetCore.Controllers;

[ApiController]
[Route("api/headers")]
public class HeadersController : ControllerBase
{
    [HttpGet]
    [Route("header")]
    public async Task<IActionResult> ReturnHeader([FromQuery(Name = "key")] string key, [FromQuery(Name = "value")] string value)
    {
        HttpContext.Response.Headers.Add(key, value);
        return Ok();
    }
}