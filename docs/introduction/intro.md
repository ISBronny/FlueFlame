<img 
    src="https://github.com/ISBronny/FlueFlame/blob/master/img/LOGO_CIRCLE.png?raw=true" 
    style="display: block;
        margin-left: auto;
        margin-right: auto;
        width: 200px;">

# What is it?

FlueFlame is an open source project for creating integration tests for ASP.NET Core applications. The main difference between FlueFlame and its analogues is the completely Fluent API. It allows you to write simple and understandable code, where every step is visible. Even a person unfamiliar with C# can understand what the test does. This will help to quickly introduce beginners and manual testers into the development of integration tests.

# Installation

Available on [NuGet](https://www.nuget.org/packages/FlueFlame.AspNet/)

#### Via Package Manager

```
NuGet\Install-Package FlueFlame.AspNet -Version 0.1.0
```

#### .NET CLI

```
dotnet add package FlueFlame.AspNet --version 0.1.0
```

#### Reference in .csproj

```
<PackageReference Include="FlueFlame.AspNet" Version="0.1.0" />
```