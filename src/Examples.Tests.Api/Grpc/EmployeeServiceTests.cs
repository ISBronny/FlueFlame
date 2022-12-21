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
			.Unary
				.Call(c => c.GetById(employee.Guid.ToGrpcModel()))
				.Response
					.AssertThat(e => e.Guid.Should().Be(employee.Guid.ToString()));
	}
	
	[Fact]
	public void GetByIdTest_DoesNotExists_ReturnNotFound()
	{
		GrpcHost
			.CreateConnection<EmployeeService.EmployeeServiceClient>()
			.Unary
				.Call(c => c.GetById(RandomGuidRequest))
				.Response
					.AssertStatusCode(StatusCode.NotFound);
	}

	[Fact]
	public void CreateEmployees_ValidEmployees_ReturnsIds()
	{
		var employees = Enumerable.Range(0, 10)
				.Select(_ => new EmployeeTestDataBuilder().Build(saveInDb: false))
				.Select(e=>e.ToGrpcModel())
		 		.ToList();
		
		GrpcHost
			.CreateConnection<EmployeeService.EmployeeServiceClient>()
			.ClientStreaming
				.Call(x=>x.CreateEmployees())
				.RequestStream
					.WriteMany(employees)
					.Complete()
				.Response
					.AssertThat(resp=>resp.Guid.Should().BeEquivalentTo(employees.Select(x=>x.Guid)));
	}

	
	[Fact]
	public void CreateEmployees_AlreadyExists_ReturnsAlreadyExists()
	{
		var employee = new EmployeeTestDataBuilder(EmployeeContext)
			.Build(saveInDb: true);
		
		GrpcHost
			.CreateConnection<EmployeeService.EmployeeServiceClient>()
			.ClientStreaming
				.Call(x=>x.CreateEmployees())
				.RequestStream
					.Write(employee.ToGrpcModel())
				.Response
					.AssertStatusCode(StatusCode.AlreadyExists);
	}
	//
	// [Fact]
	// public void GetByIdTest_DoesNotExists_ReturnsEmployee2()
	// {
	// 	var employee = new EmployeeTestDataBuilder(EmployeeContext)
	// 		.Build();
	// 	
	// 	GrpcHost
	// 		.CreateConnection<EmployeeService.EmployeeServiceClient>()
	// 		.BidirectionalStreaming(
	// 			client => client.GetByIds(),
	// 			async duplex =>
	// 			{
	// 				await duplex.RequestStream.WriteAsync(employee.Guid.ToGrpcModel());
	// 				await duplex.RequestStream.WriteAsync(RandomGuidRequest);
	// 				await duplex.RequestStream.CompleteAsync();
	// 				await duplex.ResponseStream.MoveNext();
	// 				duplex.ResponseStream.Current.Guid.Should().Be(employee.Guid.ToString());
	// 				await FluentActions.Awaiting(async () => await duplex.ResponseStream.MoveNext())
	// 					.Should()
	// 					.ThrowAsync<RpcException>();
	//
	// 			}
	// 		);
	// }
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