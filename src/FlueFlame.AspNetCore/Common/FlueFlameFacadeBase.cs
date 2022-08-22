namespace FlueFlame.AspNetCore.Common
{
    public abstract class FlueFlameFacadeBase
    {
        protected FlueFlameHost Application { get; }

        protected FlueFlameFacadeBase(FlueFlameHost application)
        {
            Application = application;
        }
    }


}