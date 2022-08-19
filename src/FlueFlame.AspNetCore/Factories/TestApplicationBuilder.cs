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
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

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
            ConfigureLogger();
        }

        private void ConfigureLogger()
        {
            Log.Logger = new LoggerConfiguration()
                //ToDo: Add Issue to Serilog - Child classes are not destructed
                // .Destructure.ByTransforming<HttpResponse>(message =>
                // {
                //     var body = new StreamReader(message.Body).ReadToEnd();
                //     return new { message.StatusCode, message.Headers, body };
                // })
                .Destructure.ByTransformingWhere<HttpRequest>(
                    t => typeof(HttpRequest).IsAssignableFrom(t),
                    message =>
                {
                    var body = new StreamReader(message.Body).ReadToEnd();
                    message.Body.Position = 0;
                    return new { Path = message.Path.Value, Query = message.QueryString.Value, message.Method, message.Headers, Body = body };
                })
                .Destructure.ByTransformingWhere<HttpResponse>(
                    t => typeof(HttpResponse).IsAssignableFrom(t),
                    message =>
                    {
                        var body = new HttpResponseBodyHelper(message).ReadAsText();
                        return new { message.StatusCode, message.Headers, Body = body };
                    })
                .WriteTo
                .Console(
                    outputTemplate:
                    "{Timestamp:HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                    theme: SystemConsoleTheme.Colored, applyThemeToRedirectedOutput: false
                )
                .CreateLogger();
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

        public FlueFlameHost Build()
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