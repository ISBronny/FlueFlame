using System.Net;
using Testing.TestData.AspNetCore.Models;

namespace Testing.Tests.AspNet.NUnit.Assertions;

public class XmlAssertionsTests : TestBase
{
    [Test]
    public void Test()
    {
        Application
            .Http.Get
            .Url("/api/large")
            .Accepts("application/xml")
            .Send()
            .HttpResponse
                .AssertStatusCode(HttpStatusCode.OK)
                .AsXml.CopyResponseTo(out LargeModel model);
                    //.AssertThat<LargeModel>(model => model.Children, Has.Count.EqualTo(10))
                    //.AssertThat<LargeModel>(model => model.Children.Select(child => child.Values), Is.All.Length.EqualTo(10));
    }
}