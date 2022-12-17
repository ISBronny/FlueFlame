using System;
using System.Net.Http;
using FlueFlame.Http.Host;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;

namespace FlueFlame.AspNetCore
{
    public class FlueFlameAspNetBuilder
    {
        public readonly HttpClient HttpClient;
        public readonly TestServer TestServer;

        private FlueFlameAspNetBuilder(HttpClient httpClient, TestServer testServer)
        {
            HttpClient = httpClient;
            TestServer = testServer;
            TestServer.CreateWebSocketClient();
        }
        
        public static FlueFlameAspNetBuilder CreateDefaultBuilder<T>(WebApplicationFactory<T> webApplicationFactory) where T : class
        {
            return new FlueFlameAspNetBuilder(webApplicationFactory.CreateClient(), webApplicationFactory.Server);
        }

        public FlueFlameAspNetBuilder ConfigureHttpClient(Action<HttpClient> configure)
        {
            configure(HttpClient);
            return this;
        }
        
        public IFlueFlameHttpHost BuildHttpHost()
        {
            return new FlueFlameHttpHostBuilder()
                .UseCustomHttpClient(HttpClient)
                .Build();
        }

        public IFlueFlameHttpHost BuildHttpHost(Action<FlueFlameHttpHostBuilder> configureBuilder)
        {
            var builder = new FlueFlameHttpHostBuilder()
                .UseCustomHttpClient(HttpClient);
            configureBuilder(builder);
            return builder.Build();
        }
    }
}