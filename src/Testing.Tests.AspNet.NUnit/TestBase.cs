using FlueFlame.AspNetCore;
using FlueFlame.AspNetCore.Factories;
using Microsoft.AspNetCore.Mvc.Testing;
using Testing.TestData.AspNetCore;

namespace Testing.Tests.AspNet.NUnit;

public class TestBase
{
    protected readonly IFlueFlameHost Application;

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

        Application = TestApplicationBuilder.CreateDefaultBuilder(webApp)
            .UseNewtonsoftJson()
            .Build();
    }
}