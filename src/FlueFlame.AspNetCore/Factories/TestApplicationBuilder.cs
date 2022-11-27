using System;
using System.IO;
using System.Net.Http;
using FlueFlame.AspNetCore.Deserialization;
using FlueFlame.AspNetCore.Services;
using FlueFlame.AspNetCore.Services.SignalR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace FlueFlame.AspNetCore.Factories
{
    public class TestApplicationBuilder
    {
        private readonly HttpClient _httpClient;
        private readonly TestServer _testServer;
        
        public readonly IHostBuilder HostBuilder;

        private TestApplicationBuilder(HttpClient httpClient, TestServer testServer)
        {
            _httpClient = httpClient;
            _testServer = testServer;
            _testServer.CreateWebSocketClient();
            HostBuilder = Host.CreateDefaultBuilder();
            RegisterServices(HostBuilder);
        }
        
        private void RegisterServices(IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(x => x.AddSingleton<IXmlSerializer, XmlSerializer>());
            hostBuilder.ConfigureServices(x => x.AddSingleton<IJsonSerializer, TextJsonJsonSerializer>());
            hostBuilder.ConfigureServices(x => x.AddScoped<HttpService>());

            hostBuilder.ConfigureServices(x => x.AddSingleton<SignalRService>());
        }

        public static TestApplicationBuilder CreateDefaultBuilder<T>(WebApplicationFactory<T> webApplicationFactory) where T : class
        {
            return new TestApplicationBuilder(webApplicationFactory.CreateClient(), webApplicationFactory.Server);
        }

        public TestApplicationBuilder ConfigureDefaultHttpClient(Action<HttpClient> configure)
        {
            configure(_httpClient);
            return this;
        }

        public IFlueFlameHost Build()
        {
            var host = HostBuilder.Build();
            return new FlueFlameHost(_httpClient, _testServer, host);
        }
    }

    public static class TestApplicationBuilderExtensions
    {
        public static TestApplicationBuilder UseNewtonsoftJson(this TestApplicationBuilder app)
        {
            app.HostBuilder.ConfigureServices(x => x.AddSingleton<IJsonSerializer, NewtonsoftJsonSerializer>());
            return app;
        }

        public static TestApplicationBuilder UseNewtonsoftJson(this TestApplicationBuilder app, JsonSerializerSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));
            app.HostBuilder.ConfigureServices(x => x.AddSingleton<IJsonSerializer, NewtonsoftJsonSerializer>(provider => new NewtonsoftJsonSerializer(settings)));
            return app;
        }

        public static TestApplicationBuilder UseTextJson(this TestApplicationBuilder app)
        {
            app.HostBuilder.ConfigureServices(x => x.TryAddSingleton<IJsonSerializer, TextJsonJsonSerializer>());
            return app;
        }
    }
}