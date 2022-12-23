using System;
using System.Collections.Generic;
using System.Net.Http;
using FlueFlame.Http.Host;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;

namespace FlueFlame.AspNetCore
{
    public class FlueFlameAspNetBuilder
    {
        private List<Action<HttpClient>> _configureClient = new();
        public HttpClient HttpClient
        {
            get
            {
                var client = TestServer.CreateClient();
                foreach (var configure in _configureClient) 
                    configure(client);
                return client;
            }
        }
        public readonly TestServer TestServer;

        private FlueFlameAspNetBuilder(TestServer testServer)
        {
            TestServer = testServer;
            //TestServer.CreateWebSocketClient();
        }

        public static FlueFlameAspNetBuilder CreateDefaultBuilder<T>(WebApplicationFactory<T> webApplicationFactory) where T : class
        {
            return new FlueFlameAspNetBuilder(webApplicationFactory.Server);
        }

        public FlueFlameAspNetBuilder ConfigureHttpClient(Action<HttpClient> configure)
        {
            _configureClient.Add(configure);
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