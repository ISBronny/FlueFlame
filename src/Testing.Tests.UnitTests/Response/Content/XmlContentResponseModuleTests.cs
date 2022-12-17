using FlueFlame.Core.Response.Content.Formatted;
using FluentAssertions;
using Testing.Tests.UnitTests.Entities;
using FlueFlame.Core.Serialization;
using FlueFlame.Http.Host;

namespace Testing.Tests.UnitTests.Response.Content;

public class XmlContentResponseModuleTests : TestBase
{
	protected string SerializedEntity =>  new XmlSerializer().SerializeObject(TestEntity);

	[Fact]
	public void AssertObject_SameObject_NotThrow()
	{
		var module = new XmlContentResponseModule<IFlueFlameHttpHost>(FlueFlameHttpHost, new XmlSerializer(), SerializedEntity);
		
		FluentActions.Invoking(() => module.AssertObject(TestEntity))
			.Should().NotThrow();
	}
	
	[Fact]
	public void AssertObject_DifferentObjects_ShouldThrow()
	{
		var module = new XmlContentResponseModule<IFlueFlameHttpHost>(FlueFlameHttpHost, new XmlSerializer(), SerializedEntity);
		
		FluentActions.Invoking(() => module.AssertObject(TestEntityHelper.Random))
			.Should().Throw<Exception>();
	}
	
	[Fact]
	public void AssertThat_Invoked()
	{
		var module = new XmlContentResponseModule<IFlueFlameHttpHost>(FlueFlameHttpHost, new XmlSerializer(), SerializedEntity);
		
		bool invoked = false;
		module.AssertThat<TestEntity>(_ => invoked = true);
		Assert.True(invoked);
	}
}