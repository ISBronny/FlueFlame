namespace FlueFlame.AspNetCore.Common
{
    public abstract class FlueFlameFacadeBase
    {
        protected IFlueFlameHost Application { get; }

        protected FlueFlameFacadeBase(IFlueFlameHost application)
        {
            Application = application;
        }
    }


}