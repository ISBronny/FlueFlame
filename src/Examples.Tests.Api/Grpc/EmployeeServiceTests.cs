using System.Net;
using Examples.Grpc;
using Examples.Tests.Api.TestDataBuilders;
using FluentAssertions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace Examples.Tests.Api.Grpc;

public class EmployeeServiceTests : IntegrationTestBase
{
	private StringValue RandomGuidRequest => new() { Value = Guid.NewGuid().ToString() };

	[Fact]
	public void GetByIdTest_Exists_ReturnsEmployee()
	{
		var employee = new EmployeeTestDataBuilder(EmployeeContext)
			.Build();
	
		GrpcHost
			.CreateConnection<EmployeeService.EmployeeServiceClient>()
			.UnaryRpc(
				client => client
					.GetById(employee.Guid.ToGrpcModel())
						.Guid.Should().Be(employee.Guid.ToString())
				);
	}
	
	[Fact]
	public void GetByIdTest_DoesNotExists_Throws()
	{
		GrpcHost
			.CreateConnection<EmployeeService.EmployeeServiceClient>()
			.UnaryRpc(
				client => FluentActions.Invoking(() => client.GetById(RandomGuidRequest))
					.Should().Throw<RpcException>().Which.StatusCode.Should().Be(StatusCode.NotFound)
			);
	}
	
	[Fact]
	public void CreateEmployees_ValidEmployees_ReturnsIds()
	{
		var employees = Enumerable.Range(0, 10)
			.Select(_ => new EmployeeTestDataBuilder().Build(saveInDb: false))
			.ToList();
		
		GrpcHost
			.CreateConnection<EmployeeService.EmployeeServiceClient>()
			.ClientStreaming(
				client => client.CreateEmployees(),
				async call =>
				{
					foreach (var e in employees)
						await call.RequestStream.WriteAsync(e.ToGrpcModel());
					await call.RequestStream.CompleteAsync();
				},
				response => response.Guid.Should().BeEquivalentTo(employees.Select(e => e.Guid.ToString()))
			);
	}
	
	[Fact]
	public void CreateEmployees_AlreadyExists_ReturnsIds()
	{
		var employees = Enumerable.Range(0, 10)
			.Select(i => new EmployeeTestDataBuilder(EmployeeContext).Build(saveInDb: i > 5))
			.ToList();
		
		GrpcHost
			.CreateConnection<EmployeeService.EmployeeServiceClient>()
			.ClientStreaming(
				client => client.CreateEmployees(),
				async call =>
				{
					var t = await FluentActions.Awaiting(async () =>
					{
						foreach (var e in employees)
							await call.RequestStream.WriteAsync(e.ToGrpcModel());
						await call.RequestStream.CompleteAsync();
					}).Should().ThrowAsync<RpcException>();
					t.Which.StatusCode.Should().Be(StatusCode.AlreadyExists);
				}
			);
	}
	
	[Fact]
	public void GetByIdTest_DoesNotExists_ReturnsEmployee2()
	{
		var employee = new EmployeeTestDataBuilder(EmployeeContext)
			.Build();
		
		GrpcHost
			.CreateConnection<EmployeeService.EmployeeServiceClient>()
			.BidirectionalStreaming(
				client => client.GetByIds(),
				async duplex =>
				{
					await duplex.RequestStream.WriteAsync(employee.Guid.ToGrpcModel());
					await duplex.RequestStream.WriteAsync(RandomGuidRequest);
					await duplex.RequestStream.CompleteAsync();
					await duplex.ResponseStream.MoveNext();
					duplex.ResponseStream.Current.Guid.Should().Be(employee.Guid.ToString());
					await FluentActions.Awaiting(async () => await duplex.ResponseStream.MoveNext())
						.Should()
						.ThrowAsync<RpcException>();

				}
			);
	}
}

public static class GrpcExtensions
{
	public static StringValue ToGrpcModel(this Guid guid) =>
		new StringValue { Value = guid.ToString() };

	public static Employee ToGrpcModel(this Domain.Models.Employee employee) =>
		new Employee()
		{
			Guid = employee.Guid.ToString(),
			Age = employee.Age,
			Position = employee.Position,
			FullName = employee.FullName
		};
}