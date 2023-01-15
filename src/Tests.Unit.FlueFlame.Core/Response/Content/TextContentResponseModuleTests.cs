using FlueFlame.Core;
using FlueFlame.Core.Response.Content;
using Moq;

namespace Tests.Unit.Core.Response.Content;

public class TextContentResponseModuleTests : TestBase
{
	protected string Text => "SomeText";
	protected IFlueFlameHost FlueFlameHost => new Mock<IFlueFlameHost>().Object;
	
	[Fact]
	public void AssertThat_Invoked()
	{
		var module = new TextContentResponseModule<IFlueFlameHost>(FlueFlameHost, Text);
		
		bool invoked = false;
		module.AssertThat(_ => invoked = true);
		Assert.True(invoked);
	}
}