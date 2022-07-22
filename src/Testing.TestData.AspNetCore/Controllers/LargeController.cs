using Microsoft.AspNetCore.Mvc;
using Testing.TestData.AspNetCore.Models;

namespace Testing.TestData.AspNetCore.Controllers;

[Route("api/large")]
[ApiController]
public class LargeController : ControllerBase
{
    private static readonly Random Random = new Random();
    
    [HttpGet]
    public IActionResult GetLargeObject()
    {
        var model = new LargeModel()
        {
            Id = 2,
            Time = DateTime.Now,
            String = RandomString(30),
            Children = Enumerable.Range(0, 10).Select(num =>
            {
                return new ChildLargeModel()
                {
                    Id = Random.Next(),
                    Values = Enumerable.Range(0, 10).Select(_ => new ValueObject()
                    {
                        Value = RandomString(15)
                    }).ToArray()
                };
            }).ToList()
        };

        return Ok(model);
    }
    
    private static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[Random.Next(s.Length)]).ToArray());
    }
}