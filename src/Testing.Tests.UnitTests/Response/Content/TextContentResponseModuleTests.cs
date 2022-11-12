using FlueFlame.AspNetCore.Modules.Response.Content;

namespace Testing.Tests.UnitTests.Response.Content;

public class TextContentResponseModuleTests : TestBase
{
	protected string Text => "SomeText";
	
	[Fact]
	public void AssertThat_Invoked()
	{
		var module = new TextContentResponseModule(FlueFlameHost, Text);
		
		bool invoked = false;
		module.AssertThat(_ => invoked = true);
		Assert.True(invoked);
	}
}