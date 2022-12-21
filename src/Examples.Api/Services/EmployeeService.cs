using Examples.Grpc;
using Examples.Infrastructure.Repositories;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace Examples.Api.Services;

public class EmployeeService : Grpc.EmployeeService.EmployeeServiceBase
{
	private readonly IEmployeeRepository _employeeRepository;

	public EmployeeService(IEmployeeRepository employeeRepository)
	{
		_employeeRepository = employeeRepository;
	}

	public override async Task<Employee> GetById(StringValue request, ServerCallContext context)
	{
		var employee = await _employeeRepository.GetById(Guid.Parse(request.Value));
		if (employee == null)
			throw new RpcException(new Status(StatusCode.NotFound, $"Employee with id {request.Value} does not exists."));
		
		return MapModel(employee);
	}

	public override async Task GetByIds(IAsyncStreamReader<StringValue> requestStream, IServerStreamWriter<Employee> responseStream, ServerCallContext context)
	{
		await foreach (var id in requestStream.ReadAllAsync())
		{
			var employee = await _employeeRepository.GetById(Guid.Parse(id.Value));
			if (employee == null)
				throw new RpcException(new Status(StatusCode.NotFound, $"Employee with id {id} does not exists."));
			await responseStream.WriteAsync(MapModel(employee));
		}
	}

	public override async Task<CreatedEmployeesIdsResponse> CreateEmployees(IAsyncStreamReader<Employee> requestStream, ServerCallContext context)
	{
		var guids = new List<Guid>();
		await foreach (var emp in requestStream.ReadAllAsync())
		{
			if (await _employeeRepository.GetById(Guid.Parse(emp.Guid)) != null)
				throw new RpcException(new Status(StatusCode.AlreadyExists, $"Employee with id {emp.Guid} already exists."));
			
			var created = await _employeeRepository.Add(MapModel(emp));
			guids.Add(created.Guid);
		}

		return new CreatedEmployeesIdsResponse()
		{
			Guid = { guids.Select(x => x.ToString()) }
		};
	}

	public override Task GetByAge(AgeRangeRequest request, IServerStreamWriter<Employee> responseStream, ServerCallContext context)
	{
		return base.GetByAge(request, responseStream, context);
	}

	private Employee MapModel(Domain.Models.Employee employee)
	{
		return new Employee()
		{
			Guid = employee.Guid.ToString(),
			Age = employee.Age,
			Position = employee.Position,
			FullName = employee.FullName
		};
	}
	
	private Domain.Models.Employee MapModel(Employee employee)
	{
		return new Domain.Models.Employee()
		{
			Guid = Guid.Parse(employee.Guid),
			Age = employee.Age,
			Position = employee.Position,
			FullName = employee.FullName
		};
	}
}