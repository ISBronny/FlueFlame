namespace FlueFlame.Core.Response.Content
{
    public sealed class TextContentResponseModule<THost> : ContentResponseModule<THost> where THost : IFlueFlameHost
    {
        public TextContentResponseModule(THost host, string content) : base(host, content)
        {
            
        }
        
        /// <summary>
        /// Copies the text of the response to a variable.
        /// </summary>
        /// <param name="response">The variable to which the body will be copied</param>
        /// <returns></returns>
        public TextContentResponseModule<THost> CopyResponseTo(out string response)
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
        public TextContentResponseModule<THost> AssertThat(Action<string> assert)
        {
            assert(Content);
            return this;
        }
    }
}