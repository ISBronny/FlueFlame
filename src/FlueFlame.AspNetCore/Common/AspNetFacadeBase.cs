namespace FlueFlame.AspNetCore.Common
{
    public abstract class AspNetModuleBase
    {
        public FlueFlameHost Application { get; }

        protected AspNetModuleBase(FlueFlameHost application)
        {
            Application = application;
        }
    }

    public abstract class AspNetService
    {

    }

    public abstract class AspNetFacadeBase
    {
        protected FlueFlameHost Application { get; }

        protected AspNetFacadeBase(FlueFlameHost application)
        {
            Application = application;
        }
    }


}