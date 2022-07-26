﻿using FlueFlame.AspNetCore.Common;
using FlueFlame.AspNetCore.Modules.Response.Content;
using FlueFlame.AspNetCore.Modules.Response.Content.Formatted;

namespace FlueFlame.AspNetCore.Modules.Response;

public abstract class ResponseModule : FlueFlameModuleBase
{
	internal ResponseModule(IFlueFlameHost application) : base(application)
	{
	}
	
	public abstract JsonContentResponseModule AsJson { get; }
	public abstract XmlContentResponseModule AsXml { get; }
	public abstract TextContentResponseModule AsText { get; }
}