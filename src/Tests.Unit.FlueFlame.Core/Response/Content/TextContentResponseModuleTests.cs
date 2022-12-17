using FlueFlame.Core;
using FlueFlame.Core.Response.Content;

namespace Tests.Unit.Core.Response.Content;

public class TextContentResponseModuleTests : TestBase
{
	protected string Text => "SomeText";
	
	[Fact]
	public void AssertThat_Invoked()
	{
		var module = new TextContentResponseModule<IFlueFlameHost>(FlueFlameHost, Text);
		
		bool invoked = false;
		module.AssertThat(_ => invoked = true);
		Assert.True(invoked);
	}
}