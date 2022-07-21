using System;
using System.Net.Http;
using FlueFlame.AspNetCore.Facades;
using FlueFlame.AspNetCore.Factories.ModuleFactories;
using FlueFlame.AspNetCore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace FlueFlame.AspNetCore
{
    public class FlueFlameHost
    {
        internal HttpClient Client { get; }
        internal TestServer TestServer { get; }
        internal ServiceFactory ServiceFactory { get; }
        internal IHost Host { get; }

        internal HttpContext HttpContext;
        internal HttpService HttpService { get; }
        


        internal FlueFlameHost(HttpClient client, TestServer testServer, IHost host)
        {
            Client = client;
            TestServer = testServer;
            Host = host;
            ServiceFactory = new ServiceFactory(Host.Services);
            HttpService = ServiceFactory.Get<HttpService>();
        }

        #region Facades

        public HttpFacade Http => new(this);

        #endregion
        
        public FlueFlameHost Run()
        {
            return this;
        }
        
        public FlueFlameHost Scenario(Action<FlueFlameHost> action)
        {
            action(this);
            return this;
        }

        internal void Send()
        {
            HttpContext = TestServer.SendAsync(httpContext =>
            {
                httpContext.Request.EnableBuffering();
                HttpService.SetupHttpContext(httpContext);
                Log.Information("Request: {@Request}", httpContext.Request);
            }).GetAwaiter().GetResult();
            Log.Information("Response: {@Response}", HttpContext.Response);
            HttpService.Reset();
        }
    }
}