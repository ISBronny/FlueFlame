using System.Net;

namespace Examples.Tests.Api.Controllers.Employees;

public class AuthorizationEmployeeControllerTests : IntegrationTestBase
{
	[Fact]
	public void Admin_HaveAccess()
	{
		HttpHost
			.Get
			.Url("api/employee")
			.WithBearerToken(GetJwtToken(role: "admin"))
			.Send()
			.Response
				.AssertStatusCode(HttpStatusCode.OK);
	}
	
	[Fact]
	public void Customer_DoesNotHaveAccess()
	{
		HttpHost
			.Get
			.Url("api/employee")
			.WithBearerToken(GetJwtToken(role: "customer"))
			.Send()
			.Response
				.AssertStatusCode(HttpStatusCode.Forbidden);
	}
}