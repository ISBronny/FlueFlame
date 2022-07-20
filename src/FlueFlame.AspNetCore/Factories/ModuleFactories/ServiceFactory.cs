using System;
using Microsoft.Extensions.DependencyInjection;

namespace FlueFlame.AspNetCore.Factories.ModuleFactories
{
    public class ServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public T Get<T>()
        {
            using var scope = _serviceProvider.CreateScope();
            return scope.ServiceProvider.GetRequiredService<T>();
        }
    }
}