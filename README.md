<p align="center"><a href="https://github.com/ISBronny/FlueFlame"><img src="https://raw.githubusercontent.com/ISBronny/FlueFlame/master/img/FlueFlameLogo.png" alt="logo" height="100"/></a></p>
<h1 align="center"><a href="https://isbronny.github.io/FlueFlame">FlueFlame</a></h1>
<p align="center">Integration testing framework for ASP.NET</p>

<p align="center">
  <a href="https://github.com/ISBronny/FlueFlame/blob/master/LICENSE">
    <img src="https://img.shields.io/github/license/ISBronny/FlueFlame?style=for-the-badge" alt="License" />
  </a>  
  <a href="https://github.com/ISBronny/FlueFlame/issues">
    <img src="https://img.shields.io/github/issues/ISBronny/FlueFlame?style=for-the-badge" alt="Issues Count" />
  </a>  
  <a href="https://www.nuget.org/packages/FlueFlame.Core/">
    <img src="https://img.shields.io/nuget/dt/FlueFlame.Core?style=for-the-badge" alt="Downloads" />
  </a>
  <a href="https://www.nuget.org/packages/FlueFlame.Core/">
    <img src="https://img.shields.io/nuget/v/FlueFlame.Core?style=for-the-badge" alt="Version" />
  </a>

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

FlueFlame is an open-source framework for creating End-To-End tests. FlueFlame was developed for testing ASP.NET Core applications, but can be used to test any backend.

It is implemented in the Fluent API style, which allows you to write understandable declarative tests. FlueFlame also has packages that allow you to test not only REST APIs, but also technologies such as gRPC.

# Getting Started

Check out the [Getting Started](https://isbronny.github.io/FlueFlame/introduction/getting-started) Documentation

# Test example

Endpoint testing that returns employees older than a certain age:

```csharp
[Test]
public void GetWithQueryReturnsOk()
{
    HttpHost.Get
        .Url("/api/employee/older-than")
        .AddQuery("olderThan", 45)
        .Send()
        .Response
            .AssertStatusCode(HttpStatusCode.OK)
            .AsJson
                .AssertThat<Employee[]>(employees => employees.Should().NotContain(x=>x.Age<45));
}

```
More examples [here](https://github.com/ISBronny/FlueFlame/tree/master/src/Examples.Tests.Api)


# License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details
