using System.Net;
using FlueFlame.AspNetCore;
using FlueFlame.AspNetCore.Factories;
using FlueFlame.AspNetCore.Modules.Http;
using FlueFlame.Extensions.Assertions.NUnit;
using Microsoft.AspNetCore.Mvc.Testing;
using Testing.TestData.AspNetCore;
using Testing.TestData.AspNetCore.Models;

namespace Testsing.Tests.AspNet.NUnit;

public class TestBase
{
    protected readonly FlueFlameHost Application;

    public TestBase()
    {
        var webApp = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {

            });
        
        Application = TestApplicationBuilder.CreateDefaultBuilder(webApp)
            .UseNewtonsoftJson()
            .Build()
            .Run();
    }
}