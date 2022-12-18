using System.Net;
using Examples.RestApi.Database;
using Examples.RestApi.Tests.TestDataBuilders;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Examples.RestApi.Tests.Controllers.Employee;

public class EmployeeControllerTests : IntegrationTestBase
{
	protected EmployeeContext EmployeeContext => ServiceProvider.CreateScope().ServiceProvider.GetRequiredService<EmployeeContext>();

	[Fact]
	public void GetByIdTest_Exists_ReturnsEmployee()
	{
		var employee = new EmployeeTestDataBuilder(EmployeeContext)
			.Build();
		
		HttpHost.Get
			.Url($"/api/employee/{employee.Guid}")
			.Send()
			.Response
				.AssertStatusCode(HttpStatusCode.OK)
				.AsJson
					.AssertThat<Models.Employee>(emp => emp.Guid.Should().Be(employee.Guid));
	}
	
	[Fact]
	public void GetByIdTest_NotExists_ReturnsNotFound()
	{
		HttpHost.Get
			.Url($"/api/employee/{Guid.NewGuid()}")
			.Send()
			.Response
				.AssertStatusCode(HttpStatusCode.NotFound);
	}
	
	[Fact]
	public void Create_ValidModel_ReturnsCreated()
	{
		var employee = new EmployeeTestDataBuilder()
			.WithAge(23)
			.WithFullName("Elon Mask")
			.Build(saveInDb: false);

		HttpHost.Post
			.Url($"/api/employee")
			.Json(employee)
			.Send()
			.Response
				.AssertStatusCode(HttpStatusCode.Created)
				.AsJson
					.AssertObject(employee);
	}
	
	[Fact]
	public void Create_AlreadyExists_ReturnsBadRequest()
	{
		var employee = new EmployeeTestDataBuilder(EmployeeContext)
			.Build(saveInDb: true);

		HttpHost.Post
			.Url($"/api/employee")
			.Json(employee)
			.Send()
			.Response
				.AssertStatusCode(HttpStatusCode.BadRequest);
	}
	
	[Fact]
	public void GetAll_NotEmpty_ReturnsAll()
	{
		var employees = Enumerable.Range(0, 10)
			.Select(_ => new EmployeeTestDataBuilder(EmployeeContext).Build(saveInDb: true))
			.ToList();

		HttpHost.Get
			.Url($"/api/employee")
			.Json(employees)
			.Send()
			.Response
				.AssertStatusCode(HttpStatusCode.OK)
				.AsJson
					.AssertThat<Models.Employee[]>(resp => resp.Should().BeEquivalentTo(employees));
	}
}