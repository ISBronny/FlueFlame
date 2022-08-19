using System.Net;
using FlueFlame.Extensions.Assertions.NUnit;
using FluentAssertions;
using Testing.TestData.AspNetCore.Models;

namespace Testing.Tests.AspNet.NUnit.Assertions;

public class JsonAssertionsTests : TestBase
{
    [Test]
    public void Test()
    {
        Application
            .Http.Get
            .Url("/api/large")
            .Send()
            .Response
                .AssertStatusCode(HttpStatusCode.OK)
                .AsJson
                    .AssertThat<LargeModel>(model => model.Children.Count.Should().Be(10))
                    .AssertThat<LargeModel>(model => model.Children.Select(child => child.Values.Length).Should().AllBeEquivalentTo(10));
    }
}