using FlueFlame.AspNetCore.Common;
using FlueFlame.AspNetCore.Deserialization;

namespace FlueFlame.AspNetCore.Modules.Response
{
    public class TextResponseModule : FormattedResponseModule
    {
        public TextResponseModule(FlueFlameHost application) : base(application)
        {
            
        }
        
        public TextResponseModule CopyResponseTo(out string response)
        {
            response = BodyHelper.ReadAsText();
            return this;
        }
    }
}