using System;

namespace FlueFlame.AspNetCore.Modules.Response.Content
{
    public sealed class TextContentResponseModule : ContentResponseModule
    {

        internal TextContentResponseModule(IFlueFlameHost application, string content) : base(application, content)
        {
            
        }
        
        /// <summary>
        /// Copies the text of the response to a variable.
        /// </summary>
        /// <param name="response">The variable to which the body will be copied</param>
        /// <returns></returns>
        public TextContentResponseModule CopyResponseTo(out string response)
        {
            response = Content;
            return this;
        }

        /// <summary>
        /// Method for working with the response body.
        /// The method is designed to test an assertion, but technically it can be used for any purpose.
        /// However, we recommend using it only for assertions.
        /// For other purposes, use <see cref="CopyResponseTo"/>.
        /// </summary>
        /// <param name="assert">Action that works with the body.</param>
        /// <returns></returns>
        public TextContentResponseModule AssertThat(Action<string> assert)
        {
            assert(Content);
            return this;
        }
    }
}