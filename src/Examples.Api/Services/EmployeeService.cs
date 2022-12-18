using Examples.Grpc;
using Examples.Infrastructure.Repositories;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace Examples.Rest.Services;

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
		return new Employee()
		{
			Guid = employee.Guid.ToString(),
			Age = employee.Age,
			Position = employee.Position,
			FullName = employee.FullName
		};
	}

	public override Task GetByIds(IAsyncStreamReader<StringValue> requestStream, IServerStreamWriter<Employee> responseStream, ServerCallContext context)
	{
		return base.GetByIds(requestStream, responseStream, context);
	}

	public override Task<CreatedEmployeesIdsResponse> CreateEmployees(IAsyncStreamReader<Employee> requestStream, ServerCallContext context)
	{
		return base.CreateEmployees(requestStream, context);
	}

	public override Task GetByAge(AgeRangeRequest request, IServerStreamWriter<Employee> responseStream, ServerCallContext context)
	{
		return base.GetByAge(request, responseStream, context);
	}
}