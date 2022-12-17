using FlueFlame.Core.Response.Content;
using FlueFlame.Http.Host;

namespace Testing.Tests.UnitTests.Response.Content;

public class TextContentResponseModuleTests : TestBase
{
	protected string Text => "SomeText";
	
	[Fact]
	public void AssertThat_Invoked()
	{
		var module = new TextContentResponseModule<IFlueFlameHttpHost>(FlueFlameHttpHost, Text);
		
		bool invoked = false;
		module.AssertThat(_ => invoked = true);
		Assert.True(invoked);
	}
}