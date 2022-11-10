using System;
using System.Collections.Generic;
using System.Net.Http;
using FlueFlame.AspNetCore.Facades;
using FlueFlame.AspNetCore.Factories.ModuleFactories;
using FlueFlame.AspNetCore.Modules.Grpc;
using FlueFlame.AspNetCore.Modules.SignalR;
using FlueFlame.AspNetCore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;

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

        internal List<HubConnection> HubConnections { get; } = new List<HubConnection>(); 

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

        public SignalRModule SignalR => new(this);
        // ReSharper disable once InconsistentNaming
        public GrpcModule gRPC => new(this);
        
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
            }).GetAwaiter().GetResult();
            HttpService.Reset();
        }
    }
}