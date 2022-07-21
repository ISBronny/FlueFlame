using System.ComponentModel.DataAnnotations;
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
        if(ModelState.IsValid)
            return Ok(await _employeeRepository.Add(employee));
        return BadRequest(ModelState);
    }
    
    [Route("all")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _employeeRepository.GetAll());
    }
    
    [Route("older-than")]
    [HttpGet]
    public async Task<IActionResult> GetByOlderThan([FromQuery(Name = "olderThan"), Range(18, 99)] int olderThan)
    {
        if(ModelState.IsValid)
            return Ok(await _employeeRepository.OlderThan(olderThan));
        return BadRequest(ModelState);
    }
}