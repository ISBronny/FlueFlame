using System.ComponentModel.DataAnnotations;
using Examples.RestApi.Models;
using Examples.RestApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Examples.RestApi.Controllers;

[ApiController]
[Route("api/employee")]
[Authorize(Roles = "admin")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeController(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    [Route("{guid:guid}")]
    [HttpGet]
    public async Task<IActionResult> GetById(Guid guid)
    {
        var employee = await _employeeRepository.GetById(guid);
        if (employee == null)
            return NotFound();
        return Ok(employee);
    }
    
    [Route("")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Employee employee)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        
        if (await _employeeRepository.GetById(employee.Guid) != null)
            return BadRequest("Already exists");
        
        var created = await _employeeRepository.Add(employee);
        await _employeeRepository.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new {guid = created.Guid}, created);



    }
    
    [Route("")]
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