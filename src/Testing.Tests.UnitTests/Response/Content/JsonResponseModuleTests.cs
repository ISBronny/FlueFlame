using FlueFlame.AspNetCore.Deserialization;
using FlueFlame.AspNetCore.Modules.Response.Content.Formatted;
using FluentAssertions;
using Newtonsoft.Json;
using Testing.Tests.UnitTests.Entities;

namespace Testing.Tests.UnitTests.Response.Content;

public class JsonContentResponseModuleTests : TestBase
{
	protected string SerializedEntity => JsonConvert.SerializeObject(TestEntity);

	[Fact]
	public void AssertObject_SameObject_NotThrow()
	{
		var module = new JsonContentResponseModule(FlueFlameHost, new NewtonsoftJsonSerializer(), SerializedEntity);
		
		FluentActions.Invoking(() => module.AssertObject(TestEntity))
			.Should().NotThrow();
	}
	
	[Fact]
	public void AssertObject_DifferentObjects_ShouldThrow()
	{
		var module = new JsonContentResponseModule(FlueFlameHost, new NewtonsoftJsonSerializer(), SerializedEntity);
		
		FluentActions.Invoking(() => module.AssertObject(TestEntityHelper.Random))
			.Should().Throw<Exception>();
	}
	
	[Fact]
	public void AssertThat_Invoked()
	{
		var module = new JsonContentResponseModule(FlueFlameHost, new NewtonsoftJsonSerializer(), SerializedEntity);
		
		bool invoked = false;
		module.AssertThat<TestEntity>(_ => invoked = true);
		Assert.True(invoked);
	}
}