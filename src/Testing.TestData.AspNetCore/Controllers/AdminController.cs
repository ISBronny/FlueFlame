using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Testing.TestData.AspNetCore.Controllers;

[ApiController]
[Route("api/admin")]
public class AdminController : ControllerBase
{
    [HttpGet]
    [Route("test")]
    [Authorize(Roles = "admin")]
    public IActionResult Test()
    {
        return Ok();
    }
    
}