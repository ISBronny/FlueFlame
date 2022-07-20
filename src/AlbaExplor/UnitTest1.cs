using Alba;

namespace AlbaExplor;

public class Application
{
    private static readonly Lazy<IAlbaHost> _host;
    static Application()
    {
        _host = new Lazy<IAlbaHost>(() => AlbaHost.For<Testing.TestData.AspNetCore.Program>(_ => { }).Result);
    }
    [SetUp]
    public void Setup()
    {
    
    }

    [Test]
    public async Task Test1()
    {
        _host.Value.AfterEach(x =>
        {
        });
    }
}