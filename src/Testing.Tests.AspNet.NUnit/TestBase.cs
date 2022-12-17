using FlueFlame.AspNetCore;
using FlueFlame.AspNetCore.Grpc;
using FlueFlame.AspNetCore.SignalR.Host;
using FlueFlame.Http.Host;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc.Testing;
using Testing.TestData.AspNetCore;

namespace Testing.Tests.AspNet.NUnit;

public class TestBase
{
    protected IFlueFlameHttpHost Http { get; }
    protected IFlueFlameGrpcHost Grpc { get; }
    protected IFlueFlameSignalRHost SignalR { get; }

    public TestBase()
    {
        var webApp = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    //Configure your services here
                });
            });

        var builder = FlueFlameAspNetBuilder.CreateDefaultBuilder(webApp);

        Http = builder.BuildHttpHost(b =>
        {
            b.ConfigureHttpClient(client =>
            {
                client.DefaultRequestHeaders.From = "Me";
            });
            b.UseTextJsonSerializer();
        });

        Grpc = builder.BuildGrpcHost(new GrpcChannelOptions()
        {
            MaxRetryAttempts = 5
        });

        SignalR = builder.BuildSignalRHost();
    }
}