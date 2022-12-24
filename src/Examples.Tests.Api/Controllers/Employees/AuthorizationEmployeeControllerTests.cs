using System.Net;

namespace Examples.Tests.Api.Controllers.Employees;

public class AuthorizationEmployeeControllerTests : TestBase
{
	[Fact]
	public void Admin_HaveAccess()
	{
		HttpHost
			.Get
			.Url("api/employee")
			.WithJwtToken(GetJwtToken(role: "admin"))
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
			.WithJwtToken(GetJwtToken(role: "customer"))
			.Send()
			.Response
				.AssertStatusCode(HttpStatusCode.Forbidden);
	}
}