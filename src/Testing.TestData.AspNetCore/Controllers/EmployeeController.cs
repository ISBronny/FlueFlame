using Microsoft.AspNetCore.Mvc;
using Testing.TestData.AspNetCore.Models;
using Testing.TestData.AspNetCore.Repositories;

namespace Testing.TestData.AspNetCore.Controllers;

[ApiController]
[Route("api/employee")]
public class EmployeeController : ControllerBase
{
    private IEmployeeRepository _employeeRepository;

    public EmployeeController(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    [Route("{guid:guid}")]
    [HttpGet]
    public async Task<IActionResult> GetById(Guid guid)
    {
        return Ok(await _employeeRepository.GetById(guid));
    }
    
    [Route("")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Employee employee)
    {
        return Ok(await _employeeRepository.Add(employee));
    }
    
    [Route("all")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _employeeRepository.GetAll());
    }
}