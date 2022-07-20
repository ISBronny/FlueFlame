using System.Net;
using FlueFlame.AspNetCore;
using FlueFlame.AspNetCore.Factories;
using FlueFlame.AspNetCore.Modules.Http;
using FlueFlame.Extensions.Assertions.NUnit;
using Microsoft.AspNetCore.Mvc.Testing;
using Testing.TestData.AspNetCore;
using Testing.TestData.AspNetCore.Models;

namespace Testsing.Tests.AspNet.NUnit;

public class Tests
{
    protected readonly FlueFlameHost Application;

    public Tests()
    {
        var webApp = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {

            });
        
        Application = TestApplicationBuilder.CreateDefaultBuilder(webApp)
            .UseNewtonsoftJson()
            .Build();
    }
    
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        var employee = new Employee()
        {
            Age = 12,
            Position = "Middle Python Developer",
            FullName = "Max Power"
        };
        
        Application.Run()
            .Http.Get
                .Url("/api/employee/all")
                .Send()
                .Response
                    .AssertStatusCode(HttpStatusCode.OK)
            .Application
                .CreateUser(employee)
                .Response
                    .AssertStatusCode(HttpStatusCode.OK)
                    .AsJson
                        .AssertThat<Employee>(e=>e.Position,Is.EqualTo(employee.Position))
                        .AssertThat<Employee>(e=>e.Age,Is.EqualTo(employee.Age))
                        .CopyResponseTo(out employee);
        
        
    }
}

public static class FlueFlameExtension
{
    public static HttpModule CreateUser(this FlueFlameHost application, Employee employee)
    {
        return application.Http.Post
            .Url("/api/employee")
            .Json(employee)
            .Send();
    }
}