using Examples.Grpc;
using Examples.Tests.Api.TestDataBuilders;
using FluentAssertions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;

namespace Examples.Tests.Api.Grpc;

public class EmployeeServiceTests : TestBase
{
	private StringValue RandomGuidRequest => new() { Value = Guid.NewGuid().ToString() };

	[Fact]
	public void GetByIdTest_Exists_ReturnsEmployee()
	{
		var employee = new EmployeeTestDataBuilder(EmployeeContext)
			.Build();

		GrpcHost
			.CreateClient<EmployeeService.EmployeeServiceClient>()
			.Unary
				.Call(c => c.GetById(employee.Guid.ToGrpcModel()))
				.Response
					.AssertThat(e => e.Guid.Should().Be(employee.Guid.ToString()));
	}
	
	[Fact]
	public void GetByIdTest_DoesNotExists_ReturnNotFound()
	{
		GrpcHost
			.CreateClient<EmployeeService.EmployeeServiceClient>()
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
			.CreateClient<EmployeeService.EmployeeServiceClient>()
			.ClientStreaming
				.Call(x=>x.CreateEmployees())
				.RequestStream
					.WriteMany(employees)
				.Response
					.AssertThat(resp=>resp.Guid.Should().BeEquivalentTo(employees.Select(x=>x.Guid)));
	}

	
	[Fact]
	public void CreateEmployees_AlreadyExists_ReturnsAlreadyExists()
	{
		var employee = new EmployeeTestDataBuilder(EmployeeContext)
			.Build(saveInDb: true);
		
		GrpcHost
			.CreateClient<EmployeeService.EmployeeServiceClient>()
			.ClientStreaming
				.Call(x=>x.CreateEmployees())
				.RequestStream
					.Write(employee.ToGrpcModel())
				.Response
					.AssertStatusCode(StatusCode.AlreadyExists);
	}
	
	[Fact]
	public void GetByIds_Exists_ReturnsEmployees()
	{
		var employees = Enumerable.Range(0, 10)
			.Select(_ => new EmployeeTestDataBuilder(EmployeeContext).Build(saveInDb: true))
			.ToList();

		GrpcHost
			.CreateClient<EmployeeService.EmployeeServiceClient>()
			.Bidirectional
			.Call(x=>x.GetByIds())
				.RequestStream
					.WriteMany(employees.Select(e=>e.Guid.ToGrpcModel()))
					.Complete()
				.ResponseStream
					.AssertForEach((resp, i) => resp.FullName.Should().Be(employees[i].FullName));
	}

	[Fact]
	public void GetByIds_SecondDoesNotExists_ReturnsNotFound()
	{
		var employee = new EmployeeTestDataBuilder(EmployeeContext)
			.Build();

		GrpcHost
			.CreateClient<EmployeeService.EmployeeServiceClient>()
			.Bidirectional
				.Call(c=>c.GetByIds())
				.RequestStream
					.Write(employee.Guid.ToGrpcModel())
					.Write(RandomGuidRequest)
				.ResponseStream
					.Next()
					.AssertCurrent(resp=>resp.FullName.Should().Be(employee.FullName))
				.RequestStream
					.Write(RandomGuidRequest)
				.ResponseStream
					.Next()
					.AssertStatusCode(StatusCode.NotFound);
	}
	
	[Fact]
	public void GetByAge_ExistsMatch_ReturnsEmployees()
	{
		var employees = Enumerable.Range(30, 30)
			.Select(age => new EmployeeTestDataBuilder(EmployeeContext)
				.WithAge(age)
				.Build(saveInDb: true))
			.Select(e=>e.ToGrpcModel())
			.ToList();

		GrpcHost
			.CreateClient<EmployeeService.EmployeeServiceClient>()
			.ServerStreaming
				.Call(x=>x.GetByAge(new AgeRangeRequest { From = 35, To = 38}))
				.ResponseStream
					.AssertForEach(e=>e.Age.Should().BeInRange(30, 38));
	}
	
	[Fact]
	public void GetByAge_EmptyMatch_ReturnsEmployees()
	{
		Enumerable.Range(30, 10)
			.Select(age => new EmployeeTestDataBuilder(EmployeeContext)
				.WithAge(age)
				.Build(saveInDb: true))
			.Select(e => e.ToGrpcModel())
			// ReSharper disable once ReturnValueOfPureMethodIsNotUsed
			.ToArray();

		GrpcHost
			.CreateClient<EmployeeService.EmployeeServiceClient>()
			.ServerStreaming
			.Call(x=>x.GetByAge(new AgeRangeRequest { From = 45, To = 55}))
				.ResponseStream
				.AssertEndOfStream();
	}
	
	[Fact]
	public void GetByAge_FromGreaterThanTo_ReturnsInvalidArguments()
	{
		GrpcHost
			.CreateClient<EmployeeService.EmployeeServiceClient>()
			.ServerStreaming
				.Call(x=>x.GetByAge(new AgeRangeRequest { From = 90, To = 10}))
				.ResponseStream
					.Next()
					.AssertStatusCode(StatusCode.InvalidArgument);
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