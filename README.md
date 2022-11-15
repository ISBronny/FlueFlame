<p align="center"><a href="https://github.com/ISBronny/FlueFlame"><img src="https://github.com/ISBronny/FlueFlame/blob/FlueFlame.Extensions.Assertions.NUnit/img/FlueFlameLogo.png" alt="logo" height="100"/></a></p>
<h1 align="center"><a href="https://isbronny.github.io/FlueFlame">FlueFlame</a></h1>
<p align="center">Integration testing framework for ASP.NET</p>

<p align="center">
  <a href="https://github.com/ISBronny/FlueFlame/blob/master/LICENSE">
    <img src="https://img.shields.io/github/license/ISBronny/FlueFlame?style=for-the-badge" alt="License" />
  </a>  
  <a href="https://github.com/ISBronny/FlueFlame/issues">
    <img src="https://img.shields.io/github/issues/ISBronny/FlueFlame?style=for-the-badge" alt="Issues Count" />
  </a>  
  <a href="https://www.nuget.org/packages/FlueFlame.AspNet/">
    <img src="https://img.shields.io/nuget/dt/FlueFlame.AspNet?style=for-the-badge" alt="Downloads" />
  </a>
  <a href="https://www.nuget.org/packages/FlueFlame.AspNet/">
    <img src="https://img.shields.io/nuget/v/FlueFlame.AspNet?style=for-the-badge" alt="Version" />
  </a>
 
  <img src="https://img.shields.io/endpoint?url=https://gist.githubusercontent.com/ISBronny/285deb1b239f73844879bdf3c9a5ea05/raw/FlueFlame__master.json&style=for-the-badge" alt="Coverage" />

</p>

<p align="center">
  <a href="https://isbronny.github.io/FlueFlame/">
    <img src="https://img.shields.io/badge/DOCUMENTATION-blueviolet?style=for-the-badge" alt="Documentation" />
  </a>
  <a href="https://github.com/users/ISBronny/projects/4/views/1">
    <img src="https://img.shields.io/badge/ROADMAP-blueviolet?style=for-the-badge" alt="Roadmap" />
  </a>  
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


More examples https://github.com/ISBronny/FlueFlame/tree/master/src/Testing.Tests.AspNet.NUnit
```

# License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details
