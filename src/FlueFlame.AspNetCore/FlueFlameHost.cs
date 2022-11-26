using System;
using System.Collections.Generic;
using System.Net.Http;
using FlueFlame.AspNetCore.Facades;
using FlueFlame.AspNetCore.Factories.ModuleFactories;
using FlueFlame.AspNetCore.Modules.Grpc;
using FlueFlame.AspNetCore.Modules.SignalR;
using FlueFlame.AspNetCore.Services;
using FlueFlame.AspNetCore.Services.SignalR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;

namespace FlueFlame.AspNetCore
{
    public interface IFlueFlameHost
    {
        public HttpFacade Http { get; }
        public SignalRModule SignalR { get; }
        public GrpcModule gRPC { get; }
        internal TestServer TestServer { get; set; }
        internal ServiceFactory ServiceFactory { get; set; }
        internal HttpService HttpService { get; set; }
        internal HttpContext HttpContext { get; set; }
        internal void Send();

    }
    
    public class FlueFlameHost : IFlueFlameHost
    {
        internal HttpClient Client { get; }
        internal TestServer TestServer => ((IFlueFlameHost)this).TestServer;
        internal HttpService HttpService => ((IFlueFlameHost)this).HttpService;
        internal ServiceFactory ServiceFactory => ((IFlueFlameHost)this).ServiceFactory;
        TestServer IFlueFlameHost.TestServer { get; set; }
        ServiceFactory IFlueFlameHost.ServiceFactory { get; set; }
        HttpService IFlueFlameHost.HttpService { get; set; }
        internal IHost Host { get; }

        HttpContext IFlueFlameHost.HttpContext { get; set; }
        

        internal List<HubConnection> HubConnections { get; } = new List<HubConnection>(); 

        internal FlueFlameHost(HttpClient client, TestServer testServer, IHost host)
        {
            Client = client;
            Host = host;
            
            ((IFlueFlameHost)this).TestServer = testServer;
            ((IFlueFlameHost)this).ServiceFactory = new ServiceFactory(Host.Services);
            ((IFlueFlameHost)this).HttpService = ServiceFactory.Get<HttpService>();
        }

        #region Facades

        public HttpFacade Http => new(this, HttpService);

        #endregion

        public SignalRModule SignalR => new(this, ServiceFactory.Get<SignalRService>());
        // ReSharper disable once InconsistentNaming
        public GrpcModule gRPC => new(this);
        
        void IFlueFlameHost.Send()
        {
            ((IFlueFlameHost)this).HttpContext = TestServer.SendAsync(httpContext =>
            {
                httpContext.Request.EnableBuffering();
                HttpService.SetupHttpContext(httpContext);
            }).GetAwaiter().GetResult();
            HttpService.Reset();
        }
    }
}