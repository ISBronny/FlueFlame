using FlueFlame.Core;
using FlueFlame.Core.Response.Content.Formatted;
using FlueFlame.Core.Serialization;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using Tests.Unit.Core.Entities;

namespace Tests.Unit.Core.Response.Content;

public class JsonContentResponseModuleTests : TestBase
{
	protected string SerializedEntity => JsonConvert.SerializeObject(TestEntity);
	protected IFlueFlameHost FlueFlameHost => new Mock<IFlueFlameHost>().Object;

	[Fact]
	public void AssertObject_SameObject_NotThrow()
	{
		var module = new JsonContentResponseModule<IFlueFlameHost>(FlueFlameHost, new TextJsonSerializer(), SerializedEntity);
		
		FluentActions.Invoking(() => module.AssertObject(TestEntity))
			.Should().NotThrow();
	}
	
	[Fact]
	public void AssertObject_DifferentObjects_ShouldThrow()
	{
		var module = new JsonContentResponseModule<IFlueFlameHost>(FlueFlameHost, new TextJsonSerializer(), SerializedEntity);
		
		FluentActions.Invoking(() => module.AssertObject(TestEntityHelper.Random))
			.Should().Throw<Exception>();
	}
	
	[Fact]
	public void AssertThat_Invoked()
	{
		var module = new JsonContentResponseModule<IFlueFlameHost>(FlueFlameHost, new TextJsonSerializer(), SerializedEntity);
		
		bool invoked = false;
		module.AssertThat<TestEntity>(_ => invoked = true);
		Assert.True(invoked);
	}
}