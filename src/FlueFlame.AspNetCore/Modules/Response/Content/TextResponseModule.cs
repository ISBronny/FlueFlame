using System;
using FlueFlame.AspNetCore.Services;
using Microsoft.AspNetCore.Http;

namespace FlueFlame.AspNetCore.Modules.Response.Content
{
    public class TextContentResponseModule : ContentResponseModule
    {

        public TextContentResponseModule(FlueFlameHost application, string content) : base(application, content)
        {
            
        }
        
        public TextContentResponseModule CopyResponseTo(out string response)
        {
            response = Content;
            return this;
        }

        public TextContentResponseModule AssertThat(Action<string> assert)
        {
            assert(Content);
            return this;
        }
    }
}