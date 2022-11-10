<p align="center"><a href="https://github.com/ISBronny/FlueFlame"><img src="https://github.com/ISBronny/FlueFlame/blob/FlueFlame.Extensions.Assertions.NUnit/img/FlueFlameLogo.png" alt="logo" height="100"/></a></p>
<h1 align="center"><a href="https://isbronny.github.io/FlueFlame">FlueFlame</a></h1>
<p align="center">Integration testing framework for ASP.NET</p>

<p align="center">
  <img src="https://img.shields.io/github/license/ISBronny/FlueFlame?style=for-the-badge" alt="License" />
  <img src="https://img.shields.io/github/issues/ISBronny/FlueFlame?style=for-the-badge" alt="Issues Count" />
  <img src="https://img.shields.io/nuget/dt/FlueFlame.AspNet?style=for-the-badge" alt="Downloads" />
  <img src="https://img.shields.io/nuget/v/FlueFlame.AspNet?style=for-the-badge" alt="Version" />
</p>

# Description

FlueFlame is an open source project for creating integration tests for ASP.NET Core applications. The main difference between FlueFlame and its analogues is the completely Fluent API. It allows you to write simple and understandable code, where every step is visible. Even a person unfamiliar with C# can understand what the test does. This will help to quickly introduce beginners and manual testers into the development of integration tests.

# Getting Started

Check out the [Quick Start](https://isbronny.github.io/FlueFlame/#/overview/quick-start) Documentation

# Test example

Endpoint testing that returns employees older than a certain age:

```csharp
[Test]
public void GetWithQueryReturnsOk()
{
    Application
        .Http.Get
        .Url("/api/employee/older-than")
        .AddQuery("olderThan", 45)
        .Send()
        .HttpResponse
            .AssertStatusCode(HttpStatusCode.OK)
            .AsJson
                .AssertThat<Employee[]>(employees => employees.Should().NotContain(x=>x.Age<45));
}

```

# License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details
