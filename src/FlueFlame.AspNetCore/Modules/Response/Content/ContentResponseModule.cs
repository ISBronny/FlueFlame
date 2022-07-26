﻿using FlueFlame.AspNetCore.Common;

namespace FlueFlame.AspNetCore.Modules.Response.Content;

public class ContentResponseModule : FlueFlameModuleBase
{
	protected string Content { get; }
	internal ContentResponseModule(IFlueFlameHost application, string content) : base(application)
	{
		Content = content;
	}
}